using EnsembleFX.Helper;
using EnsembleFX.Logging;
using EnsembleFX.Logging.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using EnsembleFX.Filters;

namespace EnsembleFX.Repository
{
    /// <summary>
    /// A MongoDB repository. Maps to a collection with the same name
    /// as type TEntity.
    /// </summary>
    /// <typeparam name="T">Entity type for this repository</typeparam>
    public class MongoDbRepository<TEntity> : IDBRepository<TEntity> where TEntity : class
    {
        private IMongoDatabase database;
        private IMongoCollection<TEntity> collection;
        private IMongoCollection<BsonDocument> bsonCollection;
        private string _collection;
        private const string COSMOS_CONFIG_SECTION = "AzureCosmosDBConnectionSettings";
        private const string COSMOS_HOST = "CosmosDB.Host";
        private const string COSMOS_PORT = "CosmosDB.Port";
        private const string COSMOS_DATABASE = "CosmosDB.Database";
        private const string COSMOS_USER_NAME = "CosmosDB.UserName";
        private const string COSMOS_PASSWORD = "CosmosDB.Password";
        private const string COSMOS_ENCRIPTION_MECHANISM = "CosmosDB.EncriptionMechanism";
        private const string COSMOS_ISCOSMOSDB = "IsCosmosDB";
        LogManager logManager;

        public MongoDbRepository(string collection, ILogController logController)
        {
            logManager = new LogManager(logController);
            _collection = collection;
            GetDatabase();
            GetCollection();
        }


        public bool Insert(TEntity entity)
        {
            try
            {
                collection.InsertOne(entity);
                return true;
            }
            catch (Exception ex)
            {
                logManager.LogMessage("Message: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + "InnerException: " + ex.InnerException, "", "", LogLevel.Error);
                return false;
            }
        }

        public bool InsertAsync(IEnumerable<TEntity> entity)
        {
            try
            {
                collection.InsertManyAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                logManager.LogMessage("Message: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + "InnerException: " + ex.InnerException, "", "", LogLevel.Error);
                return false;
            }
        }

        public bool Update(TEntity entity)
        {
            try
            {
                List<BsonElement> lstElement = entity.ToBsonDocument().Elements.ToList();
                string id = Convert.ToString(lstElement.Where(x => x.Name == "_id").FirstOrDefault().Value);
                FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", id);

                ReplaceOneResult objResult = collection.ReplaceOne(
                                               filter,
                                               entity,
                                               new UpdateOptions { IsUpsert = false }
                                       );
                return objResult.ModifiedCount > 0 ? true : false;
            }
            catch (Exception ex)
            {
                logManager.LogMessage("Message: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + "InnerException: " + ex.InnerException, "", "", LogLevel.Error);
                return false;
            }
        }

        public bool Delete(TEntity entity)
        {

            return false;
            // TODO : pending. 
            //return collection
            //    .Remove(Query.EQ("_id", entity.Id))
            //        .DocumentsAffected > 0;
        }

        public bool DeleteBy(string key, int value)
        {
            try
            {
                FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(key, value);
                DeleteResult objResult = collection.DeleteMany(filter);
                return objResult.DeletedCount > 0 ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IList<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return collection
                    .AsQueryable<TEntity>()
                        .Where(predicate.Compile())
                            .ToList();
            }
            catch (Exception ex)
            {
                logManager.LogMessage("Message: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + "InnerException: " + ex.InnerException, "", "", LogLevel.Error);
                return new List<TEntity>();
            }
        }

        public IList<TEntity> SearchFor(string key, string value)
        {
            try
            {
                FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(key, value);
                var result = collection.Find(filter).ToList();
                return result;
            }
            catch (Exception ex)
            {
                logManager.LogMessage("Message: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + "InnerException: " + ex.InnerException, "", "", LogLevel.Error);
                return new List<TEntity>();
            }
        }

        public IList<TEntity> SearchFor(string key, int value)
        {
            try
            {
                FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(key, value);
                var result = collection.Find(filter).ToList();
                return result;
            }
            catch (Exception ex)
            {
                logManager.LogMessage("Message: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + "InnerException: " + ex.InnerException, "", "", LogLevel.Error);
                return new List<TEntity>();
            }
        }

        public IList<TEntity> SearchFor(PagingFiltering criteria, int pageNo, int pageSize, out long totalCount)
        {
            try
            {
                var resultItems = new List<TEntity>();
                FilterDefinition<TEntity> filter = null;
                SortDefinition<TEntity> sortDefinition = null;
                SortDefinitionBuilder<TEntity> sortDefinitionBuilder = Builders<TEntity>.Sort;
                int searchval;
                if (null != criteria.filter)
                {

                    // TODO: Need expand query filter base on datatype.
                    if (criteria.filter.Filters != null)
                        foreach (var item in criteria.filter.Filters)
                        {
                            switch (item.Operator)
                            {
                                case "startswith":
                                    

                                    if (int.TryParse(item.Value, out searchval))
                                    {
                                        if (filter == null)
                                            filter = Builders<TEntity>.Filter.Regex(item.Field, new BsonRegularExpression(new Regex("^" + searchval)));
                                        else
                                            filter = filter & Builders<TEntity>.Filter.Regex(item.Field, new BsonRegularExpression(new Regex("^" + searchval)));
                                    }
                                    else
                                    {

                                        if (filter == null)
                                            filter = Builders<TEntity>.Filter.Regex(item.Field, new BsonRegularExpression(new Regex("^" + item.Value)));
                                        else
                                            filter = filter & Builders<TEntity>.Filter.Regex(item.Field, new BsonRegularExpression(new Regex("^" + item.Value)));
                                    }
                                    break;
                                case "eq":
                                    if (item.DataType == "int")
                                    {
                                        if (filter == null)
                                            filter = Builders<TEntity>.Filter.Eq(item.Field, item.intValue);
                                        else
                                            filter = filter & Builders<TEntity>.Filter.Eq(item.Field, item.intValue);
                                    }
                                    else
                                    {
                                        
                                        if (int.TryParse(item.Value, out searchval))
                                        {
                                            if (filter == null)
                                                filter = Builders<TEntity>.Filter.Eq(item.Field, searchval);
                                            else
                                                filter = filter & Builders<TEntity>.Filter.Eq(item.Field, searchval);
                                            
                                        }
                                        else
                                        {
                                            if (filter == null)
                                                filter = Builders<TEntity>.Filter.Eq(item.Field, item.Value);
                                            else
                                                filter = filter & Builders<TEntity>.Filter.Eq(item.Field, item.Value);
                                        }
                                        
                                    }
                                    break;
                                case "neq":
                                    if (filter == null)
                                        filter = Builders<TEntity>.Filter.Ne(item.Field, item.Value);
                                    else
                                        filter = filter & Builders<TEntity>.Filter.Ne(item.Field, item.Value);
                                    break;
                                case "contains":
                                    if (filter == null)
                                        filter = Builders<TEntity>.Filter.Regex(item.Field, "/" + item.Value + "/i");
                                    else
                                        filter = (filter | Builders<TEntity>.Filter.Regex(item.Field, "/" + item.Value + "/i"));

                                    int result = 0;
                                    //As we are not getting DataType of field in search cases so checking that the given search value is a integer. If yes adding it as a number filter as well.
                                    if ((!string.IsNullOrEmpty(item.Value)) && int.TryParse(item.Value, out result))
                                    {
                                        filter = (filter | Builders<TEntity>.Filter.Eq(item.Field, result));
                                    }

                                    break;
                                case "endswith":
                                    if (filter == null)
                                        filter = Builders<TEntity>.Filter.Regex(item.Field, new BsonRegularExpression(new Regex(item.Value + "$")));
                                    else
                                        filter = filter & Builders<TEntity>.Filter.Regex(item.Field, new BsonRegularExpression(new Regex(item.Value + "$")));
                                    break;
                                case "lt":
                                    if (filter == null)
                                        filter = Builders<TEntity>.Filter.Lt(item.Field, item.Value);
                                    else
                                        filter = filter & Builders<TEntity>.Filter.Lt(item.Field, item.Value);
                                    break;
                                case "gt":
                                    if (filter == null)
                                        filter = Builders<TEntity>.Filter.Gt(item.Field, item.Value);
                                    else
                                        filter = filter & Builders<TEntity>.Filter.Gt(item.Field, item.Value);
                                    break;
                                case "lte":
                                    if (filter == null)
                                        filter = Builders<TEntity>.Filter.Lte(item.Field, item.Value);
                                    else
                                        filter = filter & Builders<TEntity>.Filter.Lte(item.Field, item.Value);
                                    break;
                                case "gte":
                                    if (filter == null)
                                        filter = Builders<TEntity>.Filter.Gte(item.Field, item.Value);
                                    else
                                        filter = filter & Builders<TEntity>.Filter.Gte(item.Field, item.Value);
                                    break;
                                default:
                                    break;
                            }
                        }
                }

                //Added new sort block
                if (null != criteria.Sort)
                {
                    foreach (var sort in criteria.Sort)
                    {
                        if (!string.IsNullOrEmpty(sort.Field))
                        {
                            switch (sort.Dir)
                            {
                                case "asc":
                                    if (sortDefinition == null)
                                        sortDefinition = sortDefinitionBuilder.Ascending(sort.Field);
                                    else
                                        sortDefinition = sortDefinition.Ascending(sort.Field);
                                    break;
                                case "desc":
                                    if (sortDefinition == null)
                                        sortDefinition = sortDefinitionBuilder.Descending(sort.Field);
                                    else
                                        sortDefinition = sortDefinition.Descending(sort.Field);
                                    break;
                            }
                        }
                    }
                }

                
                FilterDefinition<TEntity> filterCriteria;
                if (filter == null)
                    filterCriteria = Builders<TEntity>.Filter.Empty;
                else
                    filterCriteria = filter;

                IFindFluent<TEntity, TEntity> cursor;

                if (sortDefinition == null)
                    cursor = collection.Find(filterCriteria);
                else
                    cursor = collection.Find(filterCriteria).Sort(sortDefinition);

                totalCount = cursor.Count();
                cursor = cursor.Skip(pageSize * (pageNo - 1));

                if (pageSize >= 0)
                    cursor = cursor.Limit(pageSize);

                resultItems.AddRange(cursor.ToList());
                return resultItems;
            }
            catch (Exception ex)
            {
                logManager.LogMessage("Message: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + "InnerException: " + ex.InnerException, "", "", LogLevel.Error);
                totalCount = 0;
                return new List<TEntity>();
            }
        }


        public IList<TEntity> GetAll()
        {
            try
            {
                return collection.Find(Builders<TEntity>.Filter.Empty).ToList();
            }
            catch (Exception ex)
            {
                logManager.LogMessage("Message: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + "InnerException: " + ex.InnerException, "", "", LogLevel.Error);
                return new List<TEntity>();
            }
        }

        public TEntity GetById(string id)
        {
            try
            {
                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", id);
                BsonDocument bsonDocument = bsonCollection.Find(filter).FirstOrDefault();
                var result = bsonDocument != null ? BsonSerializer.Deserialize<TEntity>(bsonDocument) : null;
                return result;
            }
            catch (Exception ex)
            {
                logManager.LogMessage("Message: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + "InnerException: " + ex.InnerException, "", "", LogLevel.Error);
                return null;
            }
        }

        public bool DeleteById(string id)
        {
            try
            {
                //FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", id);
                //DeleteResult objResult = collection.DeleteMany(filter);
                //return objResult.DeletedCount > 0 ? true : false;

                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", id);
                DeleteResult objResult = bsonCollection.DeleteMany(filter);
                return objResult.DeletedCount > 0 ? true : false;
            }
            catch (Exception ex)
            {
                logManager.LogMessage("Message: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + "InnerException: " + ex.InnerException, "", "", LogLevel.Error);
                return false;
            }
        }

        /// <summary>
        /// Delete multiple objects by key/reference
        /// </summary>
        /// <param name="key">key field</param>
        /// <param name="value">key value</param>
        /// <returns>result flag</returns>
        public bool DeleteBy(string key, string value)
        {
            try
            {
                //FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq(key, value);
                //DeleteResult objResult = collection.DeleteMany(filter);
                //return objResult.DeletedCount > 0 ? true : false;

                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq(key, value);
                DeleteResult objResult = bsonCollection.DeleteMany(filter);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Distinct search on any single column given in parameter Match
        /// Uses - BSON Document structure for Mongo DB for Match
        /// </summary>
        /// <param name="match">match values in column, case insensitive</param>
        /// purpose - aggregate query used to make search faster
        /// <returns>Distinct search result on given column</returns>
        public IList<TEntity> SearchDictinct(BsonDocument match)
        {
            return collection.Aggregate().Match(match).ToList();
        }

        /// <summary>
        /// Distinct search on any single column given in parameter Match
        /// Uses - BSON Document structure for Mongo DB for Match & Group
        /// Add projection of columns in group if to fetch only required columns
        /// </summary>
        /// <param name="match">match values in column, case insensitive</param>
        /// <param name="group">group by on columns, can also find the count</param>
        /// purpose - aggregate query used to make search faster
        /// <returns>Distinct search result on given column</returns>
        public IList<TEntity> SearchDictinctGroupBy(BsonDocument match, BsonDocument group)
        {
            var results = collection.Aggregate().Match(match).Group(group).ToList();
            return results.Select(r => BsonSerializer.Deserialize<TEntity>(r)).ToList();
        }

        public IList<TEntity> GetSelected(string query, int pageNo, int pageSize, string sortBy, string sortDirection, out long totalCount)
        {
            try
            {
                SortDefinition<TEntity> sortDefinition = null;
                SortDefinitionBuilder<TEntity> sortDefinitionBuilder = Builders<TEntity>.Sort;

                switch (sortDirection)
                {
                    case "asc":
                        if (sortDefinition == null)
                            sortDefinition = sortDefinitionBuilder.Ascending(sortBy);
                        else
                            sortDefinition = sortDefinition.Ascending(sortBy);
                        break;
                    case "desc":
                        if (sortDefinition == null)
                            sortDefinition = sortDefinitionBuilder.Descending(sortBy);
                        else
                            sortDefinition = sortDefinition.Descending(sortBy);
                        break;
                }

                var result = collection.Find(query).ToList();
                totalCount = result.Count();
                return collection.Find(query).Sort(sortDefinition).Skip(pageSize * (pageNo - 1)).Limit(pageSize).ToList();
            }
            catch (Exception ex)
            {
                totalCount = 0;
                return new List<TEntity>();
            }
        }



        public bool InsertAsync(TEntity entity)
        {
            try
            {
                collection.InsertOneAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                logManager.LogMessage("Message: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + "InnerException: " + ex.InnerException, "", "", LogLevel.Error);
                return false;
            }
        }

        public bool InsertBsonDocument(TEntity entity)
        {
            try
            {
                string strEntity = OperationHelper.ReadRequestAsString(entity);
                BsonDocument entityDocument = BsonSerializer.Deserialize<BsonDocument>(strEntity);

                bsonCollection.InsertOneAsync(entityDocument);
                return true;

            }
            catch (Exception ex)
            {
                logManager.LogMessage("Message: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + "InnerException: " + ex.InnerException, "", "", LogLevel.Error);
                return false;
            }
        }


        public bool UpdateBsonDocument(TEntity entity)
        {
            try
            {
                string strEntity = OperationHelper.ReadRequestAsString(entity);
                BsonDocument entityDocument = BsonSerializer.Deserialize<BsonDocument>(strEntity);
                List<BsonElement> lstElement = entityDocument.ToBsonDocument().Elements.ToList();
                var id = (lstElement.Where(x => x.Name == "_id").FirstOrDefault().Value);
                FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", id);


                ReplaceOneResult objResult = bsonCollection.ReplaceOne(
                                               filter,
                                               entityDocument,
                                               new UpdateOptions { IsUpsert = false }
                                       );
                return objResult.ModifiedCount > 0 ? true : false;
            }
            catch (Exception ex)
            {
                logManager.LogMessage("Message: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + "InnerException: " + ex.InnerException, "", "", LogLevel.Error);
                return false;
            }
        }

        public TEntity SearchForSingle(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return collection
                    .AsQueryable<TEntity>()
                        .Where(predicate.Compile())
                            .SingleOrDefault();
            }
            catch (Exception ex)
            {
                logManager.LogMessage("Message: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + "InnerException: " + ex.InnerException, "", "", LogLevel.Error);
                return null;
            }

        }
        #region Private Helper Methods
        private void GetDatabase()
        {
            try
            {
                MongoClient client = null;
                logManager.LogMessage("ConnectionString", "", "", LogLevel.Info);
                var connectionSettings = ConfigurationManager.GetSection(COSMOS_CONFIG_SECTION) as NameValueCollection;
                MongoClientSettings clientSettings = new MongoClientSettings();
                if (connectionSettings != null)
                {
                    // IsCosmosDB flag sets temporarily to support both mongodb and azure cosmos db databases
                    //if (connectionSettings[COSMOS_ISCOSMOSDB].ToString().Equals("false", StringComparison.OrdinalIgnoreCase))

                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CapSpecialtyMongoDBSSLEnabled"]) && ConfigurationManager.AppSettings["CapSpecialtyMongoDBSSLEnabled"].ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        clientSettings = MongoClientSettings.FromUrl(new MongoUrl(GetConnectionString()));
                        clientSettings.SslSettings = new SslSettings();
                        clientSettings.UseSsl = true;
                        client = new MongoClient(clientSettings);
                    }
                    else
                    {
                        client = new MongoClient(GetConnectionString());
                    }

                    logManager.LogMessage("GetClient()", "", "", LogLevel.Info);
                    database = client.GetDatabase(GetDatabaseName());
                    logManager.LogMessage("GetDatabase() Completed", "", "", LogLevel.Info);
                    //}
                    //else
                    //{
                    //    clientSettings.Server = new MongoServerAddress(connectionSettings[COSMOS_HOST].ToString(), int.Parse(connectionSettings[COSMOS_PORT].ToString()));
                    //    clientSettings.UseSsl = true;
                    //    clientSettings.SslSettings = new SslSettings();
                    //    clientSettings.SslSettings.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

                    //    MongoIdentity identity = new MongoInternalIdentity(connectionSettings[COSMOS_DATABASE].ToString(), connectionSettings[COSMOS_USER_NAME].ToString());
                    //    MongoIdentityEvidence evidence = new PasswordEvidence(connectionSettings[COSMOS_PASSWORD].ToString());

                    //    clientSettings.Credentials = new List<MongoCredential>()
                    //    {
                    //       new MongoCredential(connectionSettings[COSMOS_ENCRIPTION_MECHANISM].ToString(), identity, evidence)
                    //    };

                    //    client = new MongoClient(clientSettings);
                    //    logManager.LogMessage("GetClient()", "", "", LogLevel.Error);
                    //    database = client.GetDatabase(connectionSettings[COSMOS_DATABASE].ToString());
                    //    logManager.LogMessage("GetDatabase() Completed", "", "", LogLevel.Error);
                    //}
                }
            }
            catch (Exception ex)
            {
                logManager.LogMessage("Message: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + "InnerException: " + ex.InnerException, "", "", LogLevel.Error);
            }
        }

        private string GetConnectionString()
        {
            return ConfigurationManager.AppSettings.Get("DBConnectionString").Replace("{DB_NAME}", GetDatabaseName());
        }

        private string GetDatabaseName()
        {
            return ConfigurationManager.AppSettings.Get("DBDatabase");
        }

        private void GetCollection()
        {
            try
            {
                collection = database.GetCollection<TEntity>(_collection);
                bsonCollection = database.GetCollection<BsonDocument>(_collection);
            }
            catch (Exception ex)
            {
                logManager.LogMessage("Message: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + "InnerException: " + ex.InnerException, "", "", LogLevel.Error);
            }
        }

        public long GetCount()
        {
            try
            {
                return collection.Count(Builders<TEntity>.Filter.Empty);
            }
            catch (Exception ex)
            {
                logManager.LogMessage("Message: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + "InnerException: " + ex.InnerException, "", "", LogLevel.Error);
            }
            return 0;
        }

        public long SearchCountFor(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return collection.Count(predicate);
            }
            catch (Exception ex)
            {
                logManager.LogMessage("Message: " + ex.Message + Environment.NewLine + "StackTrace: " + ex.StackTrace + Environment.NewLine + "InnerException: " + ex.InnerException, "", "", LogLevel.Error);
            }
            return 0;
        }
        #endregion
    }
}
