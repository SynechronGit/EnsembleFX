//using EnsembleFX.Filters;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Repository
{
    /// <summary>
    /// A generic repository interface/contract
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity that this repository is for</typeparam>
    public interface IDBRepository<TEntity>
    {
        /// <summary>
        /// Inserts an entity into the repository and sets the entity id
        /// </summary>
        /// <param name="entity">Entity to insert</param>
        /// <returns>True if the insert has been successful otherwise false</returns>
        bool Insert(TEntity entity);
        /// <summary>
        /// Inserts an entity into the repository and sets the entity id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool InsertBsonDocument(TEntity entity);
        /// <summary>
        /// Inserts an entity into the repository asynchronously
        /// </summary>
        /// <param name="entity">Entity to insert</param>
        /// <returns>True if the insert has been successful otherwise false</returns>
        bool InsertAsync(TEntity entity);
        /// <summary>
        /// Inserts multiple entities into respository asynchronously
        /// </summary>
        /// <param name="entity">List of Entity to insert</param>
        /// <returns>True if the insert has been successful otherwise false</returns>
        bool InsertAsync(IEnumerable<TEntity> entity);
        /// <summary>
        /// Saves (updates) an entity that is already in the repository
        /// </summary>
        /// <param name="entity">Entity to update</param>
        /// <returns>True if the update was successful otherwise false</returns>
        bool Update(TEntity entity);
        /// <summary>
        /// Saves (updates) an entity that is already in the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool UpdateBsonDocument(TEntity entity);
        /// <summary>
        /// Removes an entity from the repository
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        /// <returns>True if an entity was deleted otherwise false</returns>
        bool Delete(TEntity entity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteById(string id);
        /// <summary>
        /// Delete multiple objects by key/reference
        /// </summary>
        /// <param name="key">key field</param>
        /// <param name="value">key value</param>
        /// <returns>result flag</returns>
        bool DeleteBy(string key, string value);
        /// <summary>
        /// Delete multiple objects by key/reference
        /// </summary>
        /// <param name="key">key field</param>
        /// <param name="value">key value</param>
        /// <returns></returns>
        bool DeleteBy(string key, int value);       
        /// <summary>
        /// Searches for a list of entities that match a specified predicate
        /// </summary>
        /// <param name="predicate">Predicate to use when searching for entities</param>
        /// <returns></returns>        
        IList<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// Gives count for a list of entities that match a specified predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        long SearchCountFor(Expression<Func<TEntity, bool>> predicate);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        
        //TODO add after filter project is converted
        //IList<TEntity> SearchFor(PagingFiltering criteria, int pageNo, int pageSize, out long totalCount);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IList<TEntity> SearchFor(string key, string value);
        /// <summary>
        /// Search values by integer type
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        IList<TEntity> SearchFor(string key, int value);

        /// <summary>
        /// Retrieves all the entities from the repository
        /// </summary>
        /// <returns>List of entities</returns>
        IList<TEntity> GetAll();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        long GetCount();

        /// <summary>
        /// Retrieves an entity by its integer id
        /// </summary>
        /// <param name="id">Id of the entity to retrieve</param>
        /// <returns>A matching entity with the specified id</returns>
        TEntity GetById(string id);


        /// <summary>
        /// Distinct search on any single column given in parameter Match
        /// Uses - BSON Document structure for Mongo DB for Match
        /// </summary>
        /// <param name="match">match values in column, case insensitive</param>
        /// purpose - aggregate query used to make search faster
        /// <returns>Distinct search result on given column</returns>
        IList<TEntity> SearchDictinct(BsonDocument match);


        /// <summary>
        /// Distinct search on any single column given in parameter Match
        /// Uses - BSON Document structure for Mongo DB for Match & Group
        /// Add projection of columns in group if to fetch only required columns
        /// </summary>
        /// <param name="match">match values in column, case insensitive</param>
        /// <param name="group">group by on columns, can also find the count</param>
        /// purpose - aggregate query used to make search faster
        /// <returns>Distinct search result on given column</returns>
        IList<TEntity> SearchDictinctGroupBy(BsonDocument match, BsonDocument group);

        IList<TEntity> GetSelected(string query, int pageNo, int pageSize, string sortBy, string sortDirection, out long totalCount);

        TEntity SearchForSingle(Expression<Func<TEntity, bool>> predicate);

    }
}