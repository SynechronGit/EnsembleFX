using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnsembleFX.StorageAdapter.Abstractions
{
    /// <summary>
    /// A generic repository document interface/contract. 
    /// </summary>
    /// <typeparam name="TEntity">Type of the entity that this repository is for</typeparam>
    public interface IDocumentStorageAdapter<TEntity>
    {
        /// <summary>
        /// Get list of entity of type TEntity.
        /// </summary>
        /// <param name="oDataFilterQuery">Filter query in odata format</param>
        /// <returns>The task object representing the asynchronous operation. 
        /// It returns the list of Entity of type TEntity.</returns>
        Task<IList<TEntity>> FindAsync(string oDataFilterQuery = null);

        /// <summary>
        /// Gets an entity of type TEntity for given id.
        /// </summary>
        /// <param name="id">The value representing the id of the entity to retrieve.</param>
        /// <returns>The task object representing the asynchronous operation. 
        /// It returns an entity of type TEntity.</returns>
        Task<TEntity> GetAsync<TId>(TId id);

        /// <summary>
        /// Adds the entity
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="Exceptions.Storage.CanNotInsertDocumentException"></exception>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Saves the entity.
        /// </summary>
        /// <param name="id">Id of document to update</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        /// <exception cref="Exceptions.Storage.CanNotUpdateDocumentException"></exception>
        Task SaveAsync<TId>(TId id, TEntity entity);

        /// <summary>
        /// Phyisically deletes documents
        /// </summary>
        /// <param name="id">Id of document to delete</param>
        /// <returns></returns>
        ///<exception cref="Exceptions.Storage.CanNotDeleteDocumentException"></exception>
        Task DeleteAsync<TId>(TId id);

        /// <summary>
        /// Get count of the documents as per filter
        /// </summary>
        /// <param name="oDataFilterQuery">Filter query in odata format</param>
        /// <returns>Count</returns>
        Task<long> GetCountAsync(string oDataFilterQuery = null);
    }
}
