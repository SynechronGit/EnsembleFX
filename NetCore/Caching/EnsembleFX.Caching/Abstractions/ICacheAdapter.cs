using System;
using System.Threading.Tasks;

namespace EnsembleFX.Caching.Abstractions
{
    public interface ICacheAdapter
    {
        /// <summary>
        /// Gets an item from cache 
        /// </summary>
        /// <param name="key">Unique value which references item in cache</param>
        /// <returns>Item from cache referenced with key</returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Inserts item in cache. 
        /// If object with that key already exists, overwrites the object with new one
        /// </summary>
        /// <param name="key">Unique value which references value in cache</param>
        /// <param name="value">Value of inserting item</param>
        /// <param name="timeInCache">How long inserted item will be in cache</param>
        /// <returns> <c>True</c> if item is successfully saved in cache; otherwise, <c>false</c> </returns>
        Task<bool> SetAsync<T>(string key, T value, TimeSpan? timeInCache);

        /// <summary>
        /// Removes item from cache.
        /// If the object with the specified key does not exist, the operation is ignored
        /// </summary>
        /// <param name="key">Unique value which references item in cache</param>
        /// <returns> <c>True</c> if item is successfully removed from cache; otherwise, <c>false</c></returns>
        Task<bool> RemoveAsync(string key);

        /// <summary>
        /// Removes all items from cache
        /// </summary>
        /// <returns><c>True</c> if cache is clear; otherwise, <c>false</c></returns>
        Task<bool> RemoveAll();
    }
}
