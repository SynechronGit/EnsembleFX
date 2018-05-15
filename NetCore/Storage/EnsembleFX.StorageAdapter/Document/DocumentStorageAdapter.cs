using EnsembleFX.Exceptions.Storage;
using EnsembleFX.StorageAdapter.Abstractions;
using MongoDB.Driver;
using ODataQueryHelper.Core;
using ODataQueryRunner.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnsembleFX.StorageAdapter.Document
{
    /// <summary>
    /// Document storage repository maps to a collection with the same name as type TEntity
    /// </summary>
    /// <typeparam name="TEntity">an entity</typeparam>
    public class DocumentStorageAdapter<TEntity> : IDocumentStorageAdapter<TEntity> where TEntity : class
    {
        #region Private Members
        private readonly IDocumentStorageContext<TEntity> documentStorageContext;
        private readonly IMongoDBQueryRunner<TEntity> queryRunner;
        private string Collection;

        public IDocumentStorageContext<TEntity> DocumentStorageContext => documentStorageContext;

        #endregion

        #region Constructors

        public DocumentStorageAdapter(IDocumentStorageContext<TEntity> documentStorageContext, IMongoDBQueryRunner<TEntity> queryRunner)
        {
            this.documentStorageContext = documentStorageContext;
            this.queryRunner = queryRunner;
            Collection = typeof(TEntity).Name.ToLower();
        }

        #endregion

        #region IDocumentStorageAdapter Implementation

        /// <summary>
        /// Get list of entity of type TEntity.
        /// </summary>
        /// <param name="oDataFilterQuery">Filter query in odata format</param>
        /// <returns>The task object representing the asynchronous operation. 
        /// It returns the list of Entity of type TEntity.</returns>
        public async Task<IList<TEntity>> FindAsync(string oDataFilterQuery = null)
        {
            if (string.IsNullOrEmpty(oDataFilterQuery) || queryRunner == default(IMongoDBQueryRunner<TEntity>))
            {
                return (await documentStorageContext.GetCollection(Collection).FindAsync(Builders<TEntity>.Filter.Empty))
                    .ToList();
            }
            else
            {
                try
                {
                    var queryParser = new ODataQueryParser<TEntity>();
                    var query = queryParser.TryParse(oDataFilterQuery);
                    queryRunner.Create(query);
                    return (await queryRunner.QueryAsync(documentStorageContext.GetCollection(Collection)));
                }
                catch (Exception ex)
                {
                    throw new DocumentQueryException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Gets an entity of type TEntity for given id.
        /// </summary>
        /// <param name="id">The value representing the id of the entity to retrieve.</param>
        /// <returns>The task object representing the asynchronous operation. 
        /// It returns the Entity of type TEntity.</returns>
        public async Task<TEntity> GetAsync<TId>(TId id)
        {
            //Finds the documents matching the filter by using _id field
            var result = await documentStorageContext.GetCollection(Collection).FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            var resultList = await result.ToListAsync();
            if (resultList.Count == 0)
            {
                return null;
            }

            return resultList.FirstOrDefault();
        }

        /// <summary>
        /// Adds the entity
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="DocumentInsertException"></exception>
        public async Task AddAsync(TEntity entity)
        {
            try
            {
                await documentStorageContext.GetCollection(Collection).InsertOneAsync(entity);
            }
            catch (MongoWriteException ex)
            {
                throw new CanNotInsertDocumentException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="id">Id of Document to update</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="DocumentUpdateException"></exception>
        public async Task SaveAsync<TId>(TId id, TEntity entity)
        {
            try
            {
                FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", id);

                var result = await documentStorageContext
                    .GetCollection(Collection)
                    .ReplaceOneAsync(filter, entity, new UpdateOptions { IsUpsert = false });

                if (result?.IsModifiedCountAvailable == true && result?.ModifiedCount <= 0)
                {
                    throw new CanNotUpdateDocumentException($"Unable to update {typeof(TEntity).Name}. Verify if Id is valid.");
                }
            }
            catch (MongoWriteException ex)
            {
                throw new CanNotUpdateDocumentException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="DocumentDeleteException"></exception>
        public async Task DeleteAsync<TId>(TId id)
        {
            try
            {
                var result = await documentStorageContext.GetCollection(Collection).DeleteManyAsync(Builders<TEntity>.Filter.Eq("_id", id));

                if (result?.DeletedCount <= 0 && result?.IsAcknowledged == true)
                {
                    throw new CanNotDeleteDocumentException($"Unable to delete {typeof(TEntity).Name}. Please check the Id");
                }
            }
            catch (MongoWriteException ex)
            {
                throw new CanNotDeleteDocumentException(ex.Message, ex);
            }
        }


        public async Task<long> GetCountAsync(string oDataFilterQuery = null)
        {
            if (string.IsNullOrEmpty(oDataFilterQuery) || queryRunner == default(IMongoDBQueryRunner<TEntity>))
            {
                return await documentStorageContext.GetCollection(Collection).CountAsync(Builders<TEntity>.Filter.Empty);
            }
            else
            {
                try
                {
                    var queryRunner = new MongoDBQueryRunner<TEntity>();
                    var docQuery = new ODataQueryParser<TEntity>();
                    var query = docQuery.TryParse(oDataFilterQuery);
                    queryRunner.Create(query);
                    return await documentStorageContext.GetCollection(Collection).CountAsync(queryRunner.FilterDefinition);
                }
                catch (Exception ex)
                {
                    throw new DocumentQueryException(ex.Message, ex);
                }
            }
        }

        #endregion
    }
}
