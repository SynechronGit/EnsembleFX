using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnsembleFX.StorageAdapter.Abstractions
{
    /// <summary>
    /// An interface/contract for azure table storage. 
    /// </summary>
    /// <typeparam name="T">table entity</typeparam>
    public interface ITableStorageAdapter<T> where T : TableEntity, new()
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
        
    }
}
