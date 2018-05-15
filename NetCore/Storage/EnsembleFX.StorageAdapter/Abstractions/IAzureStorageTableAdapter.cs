using EnsembleFX.Core.Filters;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace EnsembleFX.StorageAdapter.Abstractions
{
       public interface IAzureStorageTableAdapter<T> where T : TableEntity, new()
    {
        /// <summary>
        /// Gets a configuration for document storage
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetConfiguration(string key);
        /// <summary>
        /// Get all the records in the azure table storage
        /// </summary>
        /// <returns>All records</returns>
        Task<IEnumerable<T>> GetAllAsync();
        /// <summary>
        /// Initialize the azure table storage account provider and get the cloudtable reference asynchronously
        /// </summary>
        /// <param name="tableName">cloud table name</param>
        /// <returns>cloud table reference</returns>
        CloudTable Initialize(string tableName);
        /// <summary>
        /// Insert the record into azure table storage asynchronously
        /// </summary>
        /// <param name="record">table name</param>
        /// <returns>True if the insert has been successful otherwise false</returns>
        Task<bool> InsertAsync(T record);
        /// <summary>
        /// Insert the entity/table into azure table storage with its tablename asynchronously
        /// </summary>
        /// <param name="tableName">table name</param>
        /// <param name="entity">entity</param>
        /// <returns>True if the insert has been successful otherwise false</returns>
        Task<bool> InsertTableAsync(string tableName, T entity);

        bool InsertTable<T1>(string tableName, T1 entity);

        TableQuery<T1> SearchFor<T1>(CloudTable table, PagingFiltering criteria, int pageNo, int pageSize, out long totalCount) where T1 : ITableEntity, new();

        Task<IEnumerable<T>> GetAllRecordsAsync();

        Task<IEnumerable<T>> CustomFilterAsync(string customQuery);

        void Insert(T record);

        IEnumerable<T> GetSelectedTableRows(PagingFiltering filters, int takeCount, int skipCount);

        long GetTableRowCount();

    }
}
