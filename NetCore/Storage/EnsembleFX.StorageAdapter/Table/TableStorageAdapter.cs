using EnsembleFX.StorageAdapter.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnsembleFX.StorageAdapter.Table
{
    public class TableStorageAdapter<T> : ITableStorageAdapter<T> where T : TableEntity, new()
    {
        #region Internal Members

        internal string CloudConnection;
        internal string CloudContainer;

        #endregion

        #region Private Members

        private CloudStorageAccount storageAccount;
        private CloudTableClient tableClient;
        private CloudTable _cloudTable;
        private readonly IConfiguration _configuration;
        #endregion

        #region Public Members

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="tableName"></param>
        public TableStorageAdapter(IConfiguration configuration, string tableName)
        {
            _configuration = configuration;
            CloudConnection = this.GetConfiguration("TableStorageAccount:ConnectionString");
            CloudContainer = this.GetConfiguration("TableStorageAccount:CloudContainer");
            Initialize(tableName);
        }
        /// <summary>
        /// Get all documents
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync()
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

        /// <summary>
        /// Get configuration
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetConfiguration(string key)
        {
            return _configuration[key];
        }

        /// <summary>
        /// Initialize table storage client
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public CloudTable Initialize(string tableName)
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

            return _cloudTable;
        }

        /// <summary>
        /// Insert record in current table
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public async Task<bool> InsertAsync(T record)
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

            return true;
        }

        /// <summary>
        /// Insert table
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> InsertTableAsync(string tableName, T entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert((ITableEntity)entity);

            await _cloudTable.ExecuteAsync(insertOperation);

            return true;
        }        
        
        #endregion
    }
}
