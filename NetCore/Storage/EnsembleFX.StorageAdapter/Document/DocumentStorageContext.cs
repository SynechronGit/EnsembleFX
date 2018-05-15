using EnsembleFX.StorageAdapter.Abstractions;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;

namespace EnsembleFX.StorageAdapter.Document
{

    public class DocumentStorageContext<TEntity> : IDocumentStorageContext<TEntity> where TEntity : class
    {

        #region Private Members
        private readonly IConfiguration _configuration;
        private string connectionString;
        private string databaseName;
        #endregion

        #region Public Members

        public DocumentStorageContext(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = GetConfiguration("MongoDBSettings:ConnectionString");
            databaseName = GetConfiguration("MongoDBSettings:Database");
            if (!(string.IsNullOrEmpty(connectionString) && string.IsNullOrEmpty(databaseName)))
            {
                Database = GetDatabase();
                RegisterConvention();
            }
            else
            {
                throw new ArgumentNullException("ConnectionString & Database can not be null");
            }

        }

        private void RegisterConvention()
        {
            var convention = new ConventionPack();
            convention.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register("IgnoreExtraElementConvention", convention, t => true);
        }

        private IMongoDatabase Database { get; set; }

        public IMongoDatabase GetDatabase()
        {
            MongoClientSettings clientSettings = new MongoClientSettings();
            // Add logic to register configurator to log queries for troubleshooting.
            MongoClient client = new MongoClient(connectionString);
            var conventionPack = new ConventionPack();
            conventionPack.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register("IgnoreExtraElementConvention", conventionPack, t => true);
            return client.GetDatabase(databaseName);
        }

        public IMongoCollection<TEntity> GetCollection(string collectionName)
        {
            return Database.GetCollection<TEntity>(collectionName);
        }

        public string GetConfiguration(string key)
        {
            return _configuration[key];
        }
        #endregion

    }
}
