using MongoDB.Driver;

namespace EnsembleFX.StorageAdapter.Abstractions
{
    public interface IDocumentStorageContext<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets a new instance of the document database
        /// </summary>
        IMongoDatabase GetDatabase();
        
        /// <summary>
        /// Gets an instance of BsonDocument collection
        /// </summary>
        IMongoCollection<TEntity> GetCollection(string collectionName);

        /// <summary>
        /// Gets a configuration for document storage
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetConfiguration(string key);
        
    }
}
