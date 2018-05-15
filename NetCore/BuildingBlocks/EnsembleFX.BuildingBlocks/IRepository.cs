using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.BuildingBlocks
{
	public interface IRepository<T, in TId>
	{
		/// <summary>
		/// Adds Entity to repository
		/// </summary>
		/// <param name="t"></param>
		Task AddAsync(T entity);

		/// <summary>
		/// Returns all the entiteis from repository 
		/// </summary>
		/// <param name="oDataQuery">OData standard based filter query</param>
		/// <param name="includeDeleted">Set to true if you want to include deleted entity as well.</param>
		/// <returns>List of Entities of <typeparamref name="T"/></returns>
		Task<IList<T>> FindAsync(IQueryModel oDataQuery, bool includeDeleted = false);

		/// <summary>
		/// Returns Entities based on ID passed
		/// </summary>
		/// <param name="id">Guid</param>
		/// <param name="includeDeleted">Set to true if you want to include deleted entity as well.</param>
		/// <returns>T</returns>
		Task<T> GetByIdAsync(TId id, bool includeDeleted = false);

		/// <summary>
		/// Updates the entity 
		/// </summary>
		/// <param name="id">Guid</param>
		/// <param name="t">Entity to be updated</param>
		Task SaveAsync(T t);

		/// <summary>
		/// Removes the specified entity with Id
		/// </summary>
		/// <param name="id"></param>
		Task DeleteAsync(TId id);

		/// <summary>
		/// To get the count as per given filter
		/// </summary>
		/// <param name="oDataQuery"></param>
		/// <returns></returns>
		Task<long> GetCountAsync(IQueryModel oDataQuery);
	}
}
