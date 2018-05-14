using EnsembleFX.Core.Filters;
using EnsembleFX.StorageAdapter.Model;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnsembleFX.StorageAdapter
{
    public class AzureStorageTableAdapter<T> where T : TableEntity, new()
    {
        #region Internal Members

        internal string CloudConnection;
        internal string CloudContainer;

        #endregion

        #region Private Members

        private CloudStorageAccount storageAccount;
        private CloudTableClient tableClient;
        private CloudTable _cloudTable;
        #endregion

        #region Constructors

        public AzureStorageTableAdapter(IOptions<StorageAdapterAppSettings> appSettings) : this(typeof(T).Name, appSettings)
        {
        }

        public AzureStorageTableAdapter(string tableName, IOptions<StorageAdapterAppSettings> appSettings)
        {
            //TODO:: Need to configure the AppSettings in StartUp.cs file
            CloudConnection = appSettings.Value.AzureStorageAccount;
            CloudContainer = appSettings.Value.CloudContainer;
            Initialize(tableName);
        }

        #endregion

        public void Initialize(string tableName)
        {
            if (!String.IsNullOrEmpty(CloudConnection))
            {
                storageAccount = CloudStorageAccount.Parse(CloudConnection);
                tableClient = storageAccount.CreateCloudTableClient();
                _cloudTable = tableClient.GetTableReference(tableName);
                _cloudTable.CreateIfNotExistsAsync();
            }
            else
            {
                throw new InvalidOperationException("The Cloud Connection string is invalid. Cannot initialize the provider for Azure Cloud operations");
            }
        }

        public bool InsertTable<T1>(string tableName, T1 entity)
        {
            // Create the CloudTable object that represents the generic table.
            //CloudTable table = tableClient.GetTableReference(tableName);

            // Create the table if it doesn't exist.
            //table.CreateIfNotExists();

            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert((ITableEntity)entity);

            //table.Execute(insertOperation);
            _cloudTable.ExecuteAsync(insertOperation);

            return true;
        }

        //(PagingFiltering criteria, int pageNo, int pageSize, out long totalCount)
        //public IList<T> SearchFor<T>(string tableName, T entity, PagingFiltering criteria, int pageNo, int pageSize, out long totalCount)
        public TableQuery<T1> SearchFor<T1>(CloudTable table, PagingFiltering criteria, int pageNo, int pageSize, out long totalCount) where T1 : ITableEntity, new()
        {
            var resultItems = new List<DynamicTableEntity>();
            List<T1> results = new List<T1>();
            TableQuery<T1> tableQuery = new TableQuery<T1>();

            if (null != criteria.filter)
            {
                // TODO: Need expand query filter base on datatype.
                if (criteria.filter.Filters != null)
                {
                    string filter = "";
                    List<String> filterCollection = new List<string>();
                    foreach (var item in criteria.filter.Filters)
                    {
                        switch (item.Operator)
                        {
                            case "startswith":
                                break;
                            case "eq":
                                filter = filter + TableQuery.GenerateFilterCondition(item.Field, QueryComparisons.Equal, item.Value);
                                filterCollection.Add(filter);
                                //TableQuery.CombineFilters(filter);
                                break;
                            case "neq":
                                filter = filter + TableQuery.GenerateFilterCondition(item.Field, QueryComparisons.NotEqual, item.Value);
                                filterCollection.Add(filter);
                                break;
                            case "contains":
                                break;
                            case "endswith":
                                break;
                        }
                    }


                    tableQuery = new TableQuery<T1>().Where(filter);






                    //StringBuilder finalFilter = new StringBuilder();
                    //filterCollection.ForEach(delegate (string _filter)
                    //{
                    //    TableQuery.CombineFilters()
                    //});

                }

            }
            totalCount = 0;
            return tableQuery;

        }

        /// <summary>
        /// Insert an record
        /// </summary>
        /// <param name="record">The record to insert</param>
        public async Task InsertAsync(T record)
        {
            await Task.Run(() =>
            {
                if (record == null)
                {
                    throw new ArgumentNullException(nameof(record));
                }
                var operation = TableOperation.Insert(record);
                _cloudTable.ExecuteAsync(operation).ConfigureAwait(false);
            });
        }

        /// <summary>
        /// Get all the records in the table
        /// </summary>
        /// <returns>All records</returns>
        public async Task<IEnumerable<T>> GetAllRecordsAsync()
        {
            TableContinuationToken continuationToken = null;

            var query = new TableQuery<T>();

            var alllItems = new List<T>();
            do
            {
                var items = await _cloudTable.ExecuteQuerySegmentedAsync(query, continuationToken).ConfigureAwait(false);
                continuationToken = items.ContinuationToken;
                alllItems.AddRange(items);
            } while (continuationToken != null);

            return alllItems;
        }

        /// <summary>
        /// CustomFilterAsync
        /// </summary>
        /// <param name="customQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> CustomFilterAsync(string customQuery)
        {
            TableContinuationToken continuationToken = null;
            var alllItems = new List<T>();
            if (string.IsNullOrWhiteSpace(customQuery))
            {
                throw new ArgumentNullException(nameof(customQuery));
            }
            var query = new TableQuery<T>().Where(customQuery);
            do
            {
                var items = await _cloudTable.ExecuteQuerySegmentedAsync(query, continuationToken).ConfigureAwait(false); //.AsEnumerable();
                continuationToken = items.ContinuationToken;
                alllItems.AddRange(items);
            } while (continuationToken != null);

            return alllItems;
        }

        /// <summary>
        /// Insert Record
        /// </summary>
        /// <param name="record"></param>
        public void Insert(T record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }
            var operation = TableOperation.Insert(record);
            _cloudTable.ExecuteAsync(operation);
        }

        /// <summary>
        /// Gets table rows based on take count and skip count
        /// </summary>
        /// <param name="takeCount"></param>
        /// <param name="skipCount"></param>
        /// <returns></returns>
        public IEnumerable<T> GetSelectedTableRows(PagingFiltering filters, int takeCount, int skipCount)
        {
            var alllItems = new List<T>();
            skipCount = (skipCount - 1) * 10;
            var fluentQuery = new TableQuery<T>();
            //IEnumerable<T> items = new List<T>();
            //if (filters != null)
            //{
            //    if (null != filters.filter)
            //    {
            //        // TODO: Need expand query filter base on datatype.
            //        if (filters.filter.Filters != null)
            //        {
            //            string filter = "";
            //            List<String> filterCollection = new List<string>();
            //            foreach (var item in filters.filter.Filters)
            //            {
            //                switch (item.Operator)
            //                {
            //                    case "startswith":
            //                        break;
            //                    case "eq":
            //                        filter = filter + TableQuery.GenerateFilterCondition(item.Field, QueryComparisons.Equal, item.Value);
            //                        filterCollection.Add(filter);
            //                        //TableQuery.CombineFilters(filter);
            //                        break;
            //                    case "neq":
            //                        filter = filter + TableQuery.GenerateFilterCondition(item.Field, QueryComparisons.NotEqual, item.Value);
            //                        filterCollection.Add(filter);
            //                        break;
            //                    case "contains":
            //                        break;
            //                    case "endswith":
            //                        break;
            //                }
            //            }
            //            fluentQuery = new TableQuery<T>().Where(filter);
            //        }
            //    }

            //    if (filters.Sort != null)
            //    {
            //        foreach (var sortField in filters.Sort)
            //        {
            //            items = _cloudTable.ExecuteQuery(fluentQuery).OrderBy(m => sortField.Field.GetType()).Skip(skipCount).Take(takeCount);
            //        }
            //    }
            //    else
            //    {
            //        items = _cloudTable.ExecuteQuery(fluentQuery).Skip(skipCount).Take(takeCount);
            //    }
            //}

            TableContinuationToken continuationToken = new TableContinuationToken();
            var items = _cloudTable.ExecuteQuerySegmentedAsync(fluentQuery, continuationToken).Result.Skip(skipCount).Take(takeCount);
            alllItems.AddRange(items);
            return alllItems;
        }
        /// <summary>
        /// Gets total number of rows in a table
        /// </summary>
        /// <returns></returns>
        public long GetTableRowCount()
        {
            TableContinuationToken continuationToken = new TableContinuationToken();
            return _cloudTable.ExecuteQuerySegmentedAsync(new TableQuery<T>(), continuationToken).Result.LongCount();
        }

    }
}
