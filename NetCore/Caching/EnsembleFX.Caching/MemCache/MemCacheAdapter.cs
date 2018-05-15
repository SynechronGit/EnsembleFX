using EnsembleFX.Caching.Abstractions;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using Enyim.Caching.Memcached.Results;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace EnsembleFX.Caching.MemCache
{
    public class MemCacheAdapter : ICacheAdapter
    {
        #region Private members

        private IConfiguration configuration;
        private IMemcachedClient memcachedClient;

        #endregion

        #region Public properties

        /// <summary>
        /// Connection string for cache instance
        /// </summary>
        public string CacheConnectionString { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemCacheAdapter"/> class.
        /// </summary>
        /// <param name="configuration">Configuration which is going to be used for Memcached</param>
        /// <param name="memcachedClient">Client which has methods for manipulating data in cache</param>
        /// <param name="databaseRepository">Client which has methods for manipulating data from Storage component</param>
        public MemCacheAdapter(IConfiguration configuration, IMemcachedClient memcachedClient)
        {
            this.configuration = configuration;
            this.memcachedClient = memcachedClient;
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
            this.Validation(this.memcachedClient, key, "");

            try
            {
                var result = await this.memcachedClient.GetAsync<T>(key); //No overload for method 'GetAsync' takes 2 arguments.

                return result.Value;
            }
            catch (Exception e)
            {
                //TODO : Log exception
                return default(T);
            }
        }

        /// <summary>
        /// Gets a configuration for cache
        /// </summary>
        /// <param name="namePath">Path in appsetting.json to specific parameters</param>
        /// <returns>Configurations parameters</returns>
        public string GetConfiguration(string namePath)
        {
            return this.configuration[namePath];
        }

        /// <summary>
        /// Removes item from cache
        /// </summary>
        /// <param name="key">Unique value which references item in cache</param>
        /// <returns> <c>True</c> if item is successfully removed from cache; otherwise, <c>false</c></returns>
        public async Task<bool> RemoveAsync(string key)
        {
            this.Validation(this.memcachedClient, key, "");

            try
            {
                var result = await this.memcachedClient.RemoveAsync(key);//No overload for method 'RemoveAsync' takes 2 arguments.
                return result;
            }
            catch (Exception e)
            {
                //TODO : Log exception
                return false;
            }
        }

        /// <summary>
        /// Removes all items from cache
        /// </summary>
        /// <returns><c>True</c> if cache is clear; otherwise, <c>false</c></returns>
        public async Task<bool> RemoveAll()
        {
            this.Validation(this.memcachedClient, "", "");

            try
            {
               await this.memcachedClient.FlushAllAsync(); //FlushAllAsync return type is void
                return true;
            }
            catch (Exception e)
            {
                //TODO : Log exception
                return false;
            }
        }

        /// <summary>
        /// Inserts item in cache
        /// </summary>
        /// <param name="key">Unique value which references value in cache. 
        /// If you want to avoid collision, please use key with useful string. For example: "slash:1", "object:32323"</param>
        /// <param name="value">Value of inserting item</param>
        /// <param name="timeInCache">How long inserted item will be in cache</param>
        /// <returns> <c>True</c> if item is successfully saved in cache; otherwise, <c>false</c> </returns>
        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? timeInCache)
        {
            //IOperationResult result = null;

            this.Validation(this.memcachedClient, key, value);

            try
            {
               var result = await this.memcachedClient.StoreAsync(StoreMode.Set, key, value, timeInCache.GetValueOrDefault()); //No overload for method 'RemoveAsync' takes 2 arguments.

                //if (result == null) // Also return type is direclty bool instead of IOperationResult
                //{
                //    return false;
                //}

                //return result.Success;

                return result;
            }
            catch (Exception e)
            {
                //TODO : Log exception
                return false;
            }
        }

        #endregion

        #region Private methods
        private void Validation(IMemcachedClient cacheDatabase, string key, object value)
        {
            if (cacheDatabase == null)
            {
                throw new InvalidOperationException("The cache database was not initialized or the provider operation has failed");
            }

            if (key == null || value == null)
            {
                throw new ArgumentNullException("Key parameter or object value parameter can not be null");
            }
        }

        #endregion
    }
}
