
namespace EnsembleFX.DataFrame.Adapters
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Dynamic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    //public class AzureTableConnector<T> : IDataSetConnector where T : TableEntity, new()
    public class AzureTableAdapter : TableEntity, IAdapter
    {
        private CloudTable _cloudTable;
        public AzureTableAdapter(AzureTableAdapterConfiguration configuration)
        {
            Configuration = configuration;
            InitializeCloudClient();

        }



        public Type GetConfigurationType()
        {
            return typeof(AzureTableAdapterConfiguration);
        }

        public void Initialize(IAdapterConfiguration config)
        {
            Configuration = (AzureTableAdapterConfiguration)config;
        }

        private void InitializeCloudClient()
        {
            try
            {
                if (String.IsNullOrWhiteSpace(Configuration.StorageConnectionString) && String.IsNullOrWhiteSpace(Configuration.Name))
                {
                    throw new InvalidOperationException("The cloud client was not initialized");
                }
                var cloudTableClient = CreateTableClient(Configuration.StorageConnectionString);
                _cloudTable = cloudTableClient.GetTableReference(Configuration.Name);
                CreateTable();
                // todo added if not pass configuarton get default config from app.config
            }
            catch (Exception)
            {

                throw;
            }

        }

        public AzureTableAdapterConfiguration Configuration { get; set; }

        private static CloudTableClient CreateTableClient(string connectionString)
        {

            var cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            return cloudTableClient;

        }

        public async void CreateTable()
        {
            await _cloudTable.CreateIfNotExistsAsync();
        }

        public void Write(DataSet data)
        {

            string partitionKey = string.Empty;
            string rowKey = string.Empty;
            DataTable dataTable = data.Tables[Configuration.Name];
            // var list = new List<dynamic>();
            foreach (DataRow row in dataTable.Rows)
            {
                partitionKey = string.Empty;
                rowKey = string.Empty;
                Dictionary<string, EntityProperty> obj = new Dictionary<string, EntityProperty>();
                foreach (DataColumn column in row.Table.Columns)
                {
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                    obj.Add(column.ColumnName, new EntityProperty(textInfo.ToTitleCase(Convert.ToString(row[column.ColumnName]))));
                    if (!string.IsNullOrEmpty(Configuration.PartitionKey) && Configuration.PartitionKey == column.ColumnName)
                    {
                        partitionKey = Convert.ToString(row[column.ColumnName]);
                    }

                    if (!string.IsNullOrEmpty(Configuration.RowKey) && Configuration.RowKey == column.ColumnName)
                    {
                        rowKey = Convert.ToString(row[column.ColumnName]);
                    }

                }

                partitionKey = string.IsNullOrWhiteSpace(partitionKey) ? Convert.ToString(Guid.NewGuid()) : partitionKey;
                rowKey = string.IsNullOrWhiteSpace(rowKey) ? Convert.ToString(Guid.NewGuid()) : rowKey;

                var entity = new DynamicTableEntity(partitionKey, rowKey, "*", obj);
                //  _cloudTable.Execute(TableOperation.InsertOrReplace(entity));

                //list.Add(entity);
                Insert(entity);
            }

            //BatchInsert(list);

        }


        /// <summary>
        /// Insert an record
        /// </summary>
        /// <param name="record">The record to insert</param>
        private async void Insert(dynamic record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var operation = TableOperation.InsertOrMerge(record);
            await _cloudTable.ExecuteAsync(operation);
        }

        /// <summary>
        /// Insert multiple records
        /// </summary>
        /// <param name="records">The records to insert</param>
        //private void BatchInsert(List<dynamic> records)
        //{
        //    try
        //    {
        //        if (records == null)
        //        {
        //            throw new ArgumentNullException(nameof(records));
        //        }

        //        var partitionSeparation = records.GroupBy(x => x.PartitionKey)
        //            .OrderBy(g => g.Key)
        //            .Select(g => g.ToList());

        //        foreach (var entry in partitionSeparation)
        //        {
        //            var operation = new TableBatchOperation();
        //            entry.ForEach(operation.Insert);

        //            if (operation.Any())
        //            {
        //                _cloudTable.ExecuteBatch(operation);
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public DataSet Read()
        {

            DataSet dataSet = new DataSet();
            DataTable datatable = new DataTable();
            datatable.TableName = Configuration.Name;
            IEnumerable<DynamicTableEntity> allrecoreds = GetAllRecords().Result;
            foreach (var item in allrecoreds)
            {
                datatable.Columns.Add("PartitionKey");
                datatable.Columns.Add("RowKey");
                foreach (var prop in item.Properties)
                {
                    datatable.Columns.Add(prop.Key);
                }
                break;
            }

            foreach (var item in allrecoreds)
            {
                DataRow dr = datatable.NewRow();
                dr["PartitionKey"] = item.PartitionKey;
                dr["RowKey"] = item.RowKey;
                foreach (var prop in item.Properties)
                {
                    dr[prop.Key] = prop.Value.StringValue;
                }
                datatable.Rows.Add(dr);
            }

            dataSet.Tables.Add(datatable);

            return dataSet;

        }



        /// <summary>
        /// Get all the records in the table
        /// </summary>
        /// <returns>All records</returns>
        private async Task<IEnumerable<DynamicTableEntity>> GetAllRecords()
        {
            TableContinuationToken continuationToken = null;
            dynamic results = null;
            do
            {
                results = await _cloudTable.ExecuteQuerySegmentedAsync(new TableQuery(), continuationToken);
                continuationToken = results.ContinuationToken;
            } while (continuationToken != null);
            return results;
        }


    }
}
