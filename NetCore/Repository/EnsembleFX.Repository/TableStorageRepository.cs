using EnsembleFX.Repository.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Repository
{
    public class TableStorageRepository<T> : ITableStorageRepository<T> where T : TableEntity, new()
    {

        /// <summary>
        /// The cloud table
        /// </summary>
        private readonly CloudTable _cloudTable;

        /// <summary>
        /// The default retries
        /// </summary>
        private const int DefaultRetries = 3;

        /// <summary>
        /// The default retry in seconds
        /// </summary>
        private const double DefaultRetryTimeInSeconds = 1;

        #region Construction

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tableName">The table name</param>
        /// <param name="storageConnectionString">The connection string</param>
        public TableStorageRepository(AzureConnectionString azureConnectionString)
             : this(typeof(T).Name, azureConnectionString.ConnectionString, DefaultRetries, DefaultRetryTimeInSeconds) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tableName">The table name</param>
        /// <param name="storageConnectionString">The connection string</param>
        /// <param name="retries">Number of retries</param>
        /// <param name="retryWaitTimeInSeconds">Wait time between retries in seconds</param>
        public TableStorageRepository(string tableName, string storageConnectionString, int retries, double retryWaitTimeInSeconds)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (string.IsNullOrWhiteSpace(storageConnectionString))
            {
                throw new ArgumentNullException(nameof(storageConnectionString));
            }

            OptimisePerformance(storageConnectionString);


            var cloudTableClient = CreateTableClient(storageConnectionString, retries, retryWaitTimeInSeconds);

            _cloudTable = cloudTableClient.GetTableReference(tableName);
            CreateTableAsync().Wait();
        }

        /// <summary>
        /// Settings to improve performance
        /// </summary>
        private static void OptimisePerformance(string storageConnectionString)
        {
            var account = CloudStorageAccount.Parse(storageConnectionString);
            var tableServicePoint = ServicePointManager.FindServicePoint(account.TableEndpoint);
            tableServicePoint.UseNagleAlgorithm = false;
            tableServicePoint.Expect100Continue = false;
        }

        /// <summary>
        /// Create the table client
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <param name="retries">Number of retries</param>
        /// <param name="retryWaitTimeInSeconds">Wait time between retries in seconds</param>
        /// <returns>The table client</returns>
        private static CloudTableClient CreateTableClient(string connectionString, int retries, double retryWaitTimeInSeconds)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(connectionString);

            var requestOptions = new TableRequestOptions
            {
                RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(retryWaitTimeInSeconds), retries)
            };

            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            cloudTableClient.DefaultRequestOptions = requestOptions;
            return cloudTableClient;
        }

        #endregion Constructor        

        #region Asynchronous Methods

        public async Task<List<T>> CustomFilter(string customQuery)
        {
            TableContinuationToken continuationToken = null;
            if (string.IsNullOrWhiteSpace(customQuery))
            {
                throw new ArgumentNullException(nameof(customQuery));
            }

            var query = new TableQuery<T>().Where(customQuery);

            var items = await _cloudTable.ExecuteQuerySegmentedAsync(query, continuationToken);

            return items.ToList();
        }

        public async Task<List<T>> DistinctCustomFilter(string customQuery)
        {
            TableContinuationToken continuationToken = null;
            if (string.IsNullOrWhiteSpace(customQuery))
            {
                throw new ArgumentNullException(nameof(customQuery));
            }

            var query = new TableQuery<T>().Where(customQuery);

            var items = await _cloudTable.ExecuteQuerySegmentedAsync(query, continuationToken);

            return items.Distinct().ToList();
        }

        /// <summary>
        /// Create the table
        /// </summary>
        public async Task CreateTableAsync()
        {
            await _cloudTable.CreateIfNotExistsAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Does the table exist
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TableExistsAsync()
        {
            return await _cloudTable.ExistsAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Insert an record
        /// </summary>
        /// <param name="record">The record to insert</param>
        public async Task InsertAsync(T record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var operation = TableOperation.Insert(record);

            await _cloudTable.ExecuteAsync(operation).ConfigureAwait(false);
        }

        /// <summary>
        /// Insert multiple records
        /// </summary>
        /// <param name="records">The records to insert</param>
        public async Task InsertAsync(IEnumerable<T> records)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            var partitionSeparation = records.GroupBy(x => x.PartitionKey)
           .OrderBy(g => g.Key)
           .Select(g => g.ToList());

            foreach (var entry in partitionSeparation)
            {
                var operation = new TableBatchOperation();
                entry.ForEach(operation.Insert);

                if (operation.Any())
                {
                    await _cloudTable.ExecuteBatchAsync(operation).ConfigureAwait(false);
                }
            }
        }

        public async Task InsertOrMerge(T record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var operation = TableOperation.InsertOrMerge(record);
            await _cloudTable.ExecuteAsync(operation);
        }

        public async Task InsertOrMerge(IEnumerable<T> records)
        {
            if (records == null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            var partitionSeparation = records.GroupBy(x => x.PartitionKey)
                .OrderBy(g => g.Key)
                .Select(g => g.ToList());

            foreach (var entry in partitionSeparation)
            {
                var operation = new TableBatchOperation();
                entry.ForEach(operation.InsertOrMerge);

                if (operation.Any())
                {
                    await _cloudTable.ExecuteBatchAsync(operation);
                }
            }
        }


        /// <summary>
        /// Update an record
        /// </summary>
        /// <param name="record">The record to update</param>
        public async Task UpdateAsync(T record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var operation = TableOperation.Merge(record);

            await _cloudTable.ExecuteAsync(operation).ConfigureAwait(false);
        }

        /// <summary>
        /// Update an record using the wildcard etag
        /// </summary>
        /// <param name="record">The record to update</param>
        public async Task UpdateUsingWildcardEtagAsync(T record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            record.ETag = "*";
            await UpdateAsync(record).ConfigureAwait(false);
        }


        /// <summary>
        /// Update an entry
        /// </summary>
        /// <param name="record">The record to update</param>
        public async Task DeleteAsync(T record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var operation = TableOperation.Delete(record);

            await _cloudTable.ExecuteAsync(operation).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete a record using the wildcard etag
        /// </summary>
        /// <param name="record">The record to delete</param>
        public async Task DeleteUsingWildcardEtagAsync(T record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            record.ETag = "*";

            await DeleteAsync(record).ConfigureAwait(false);
        }


        /// <summary>
        /// Delete the table
        /// </summary>
        public async Task DeleteTableAsync()
        {
            await _cloudTable.DeleteIfExistsAsync();
        }

        /// <summary>
        /// Get an record by partition and row key
        /// </summary>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <returns>The record found or null if not found</returns>
        public async Task<T> GetRecordAsync(string partitionKey, string rowKey)
        {
            if (string.IsNullOrWhiteSpace(partitionKey))
            {
                throw new ArgumentNullException(nameof(partitionKey));
            }

            if (string.IsNullOrWhiteSpace(rowKey))
            {
                throw new ArgumentNullException(nameof(rowKey));
            }

            // Create a retrieve operation that takes a customer record.
            var retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            // Execute the operation.
            var retrievedResult = await _cloudTable.ExecuteAsync(retrieveOperation).ConfigureAwait(false);

            return retrievedResult.Result as T;
        }

        /// <summary>
        /// Get the records by partition key
        /// </summary>
        /// <param name="partitionKey">The partition key</param>
        /// <returns>The records found</returns>
        public async Task<IEnumerable<T>> GetByPartitionKeyAsync(string partitionKey)
        {
            if (string.IsNullOrWhiteSpace(partitionKey))
            {
                throw new ArgumentNullException(nameof(partitionKey));
            }

            TableContinuationToken continuationToken = null;

            var query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            var allItems = new List<T>();
            do
            {
                var items = await _cloudTable.ExecuteQuerySegmentedAsync(query, continuationToken).ConfigureAwait(false);
                continuationToken = items.ContinuationToken;
                allItems.AddRange(items);
            } while (continuationToken != null);

            return allItems;
        }

        /// <summary>
        /// Get the records by row key
        /// </summary>
        /// <param name="rowKey">The row key</param>
        /// <returns>The records found</returns>
        public async Task<IEnumerable<T>> GetByRowKeyAsync(string rowKey)
        {
            if (string.IsNullOrWhiteSpace(rowKey))
            {
                throw new ArgumentNullException(nameof(rowKey));
            }

            TableContinuationToken continuationToken = null;

            var query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey));

            var allItems = new List<T>();
            do
            {
                var items = await _cloudTable.ExecuteQuerySegmentedAsync(query, continuationToken).ConfigureAwait(false);
                continuationToken = items.ContinuationToken;
                allItems.AddRange(items);
            } while (continuationToken != null);

            return allItems;
        }

        /// <summary>
        /// Get all the records in the table
        /// </summary>
        /// <returns>All records</returns>
        public async Task<IEnumerable<T>> GetAllRecordsAsync()
        {
            TableContinuationToken continuationToken = null;

            var query = new TableQuery<T>();

            var allItems = new List<T>();
            do
            {
                var items = await _cloudTable.ExecuteQuerySegmentedAsync(query, continuationToken).ConfigureAwait(false);
                continuationToken = items.ContinuationToken;
                allItems.AddRange(items);
            } while (continuationToken != null);

            return allItems;
        }



        #endregion Asynchronous Methods

    }
}
