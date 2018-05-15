using EnsembleFX.Caching.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Caching.RedisCache
{
    public class RedisCacheAdapter : ICacheAdapter
    {

        #region Private members

        private readonly IRedisCacheProvider provider;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheAdapter"/> class.
        /// </summary>
        /// <param name="configuration">Configuration from redis</param>
        public RedisCacheAdapter(IRedisCacheProvider cacheProvider)
        {
            this.provider = cacheProvider ?? throw new ArgumentNullException(nameof(cacheProvider), "Cannot be null");
        }

        #endregion

        #region ICacheAdapter Implementation

        /// <summary>
        /// Gets an item from cache 
        /// </summary>
        /// <param name="key">Unique value which references item in cache</param>
        /// <returns>Item from cache referenced with key</returns>
        public async Task<T> GetAsync<T>(string key)
        {
            var dataBase = provider.CreateConnection()
               .GetCacheDataBase();
            Validate(key, string.Empty);

            try
            {
                return await dataBase.GetAsync<T>(key);
            }
            catch (Exception e)
            {
                //TODO : Log exception
                return default(T);
            }
        }

        /// <summary>
        /// Removes item from cache.
        /// If the object with the specified key does not exist, the operation is ignored
        /// </summary>
        /// <param name="key">Unique value which references item in cache</param>
        /// <returns> <c>True</c> if item is successfully removed from cache; otherwise, <c>false</c></returns>
        public async Task<bool> RemoveAsync(string key)
        {
            var dataBase = provider.CreateConnection()
                .GetCacheDataBase();
            Validate(key, string.Empty);

            try
            {
                return await dataBase.KeyDeleteAsync(key);
            }
            catch (Exception e)
            {
                //TODO : Log exception
                return false;
            }
        }

        /// <summary>
        /// Removes all items from cache.
        /// This method is not supported
        /// </summary>
        /// <returns><c>True</c> if cache is clear; otherwise, <c>false</c></returns>
        public Task<bool> RemoveAll()
        {
            throw new NotImplementedException("The Azure Redis Cache Adapter does not support this feature. Only in Redis-CLI");
        }

        /// <summary>
        /// Inserts item in cache.
        /// If object with that key already exists, overwrites the object with new one
        /// </summary>
        /// <param name="key">Unique value which references value in cache. 
        /// If you want to avoid collision, please use key with useful string. For example: "slash:1", "object:32323"</param>
        /// <param name="value">Value of inserting item</param>
        /// <param name="timeInCache">How long inserted item will be in cache</param>
        /// <returns> <c>True</c> if item is successfully saved in cache; otherwise, <c>false</c> </returns>
        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? timeInCache)
        {
            var dataBase = provider.CreateConnection()
                .GetCacheDataBase();
            Validate(key, value);

            try
            {
                return await dataBase.SetAsync(key, value, timeInCache);
            }
            catch (Exception e)
            {
                //TODO : Log exception
                return false;
            }
        }

        #endregion

        #region Private methods
        private void Validate(string key, object value)
        {
            if (key == null || value == null)
            {
                throw new ArgumentNullException("Key parameter or object value parameter can not be null");
            }
        }
        #endregion
    }
}
