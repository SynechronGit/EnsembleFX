using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
namespace EnsembleFX.Caching.RedisCache
{
    public static class RedisCacheExtensions
    {
        #region Public methods

        /// <summary>
        /// Extension generic method for getting data from cache
        /// </summary>
        /// <typeparam name="T"> Type of object we want to be delivered from cache</typeparam>
        /// <param name="cache"> Interface which we are extending</param>
        /// <param name="key"> Unique value which references value in cache. 
        /// If you want to avoid collision, please use key with useful string. For example: "slash:1", "object:32323"</param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(this IDatabase cache, string key)
        {
            return Deserialize<T>(await cache.StringGetAsync(key));
        }

        /// <summary>
        /// Inserts item in cache
        /// </summary>
        /// <param name="cache"> Interface which we are extending</param>
        /// <param name="key"> Unique value which references value in cache. 
        /// If you want to avoid collision, please use key with useful string. For example: "slash:1", "object:32323"</param>
        /// <param name="value"> Value of inserting item</param>
        /// <param name="timeToLive"> How long inserted item will be in cache</param>
        /// <returns><c>True</c> if item is successfully saved in cache; otherwise, <c>false</c></returns>
        public static async Task<bool> SetAsync<T>(this IDatabase cache, string key, T value, TimeSpan? timeToLive)
        {
            if (timeToLive.HasValue)
            {
                return await cache.StringSetAsync(key, Serialize(value), timeToLive);
            }
            else
            {
                return await cache.StringSetAsync(key, Serialize(value));
            }
        }

        #endregion

        #region Private helper methods

        /// <summary>
        /// Serialize object into stream
        /// </summary>
        /// <param name="o">Object we want to serialize</param>
        /// <returns>Object serialized into array of bytes</returns>
        static string Serialize(object o)
        {
            string result = string.Empty;

            if (o != null)
            {
                result = JsonConvert.SerializeObject(o);
            }

            return result;
        }

        /// <summary>
        /// Deserialize stream back into object
        /// </summary>
        /// <typeparam name="T">Type of object we want stream to be deserialized</typeparam>
        /// <param name="stream">Object as array of bytes</param>
        /// <returns>Deserialize object</returns>
        static T Deserialize<T>(string val)
        {
            T result = default(T);

            if (!string.IsNullOrEmpty(val))
            {
                result = JsonConvert.DeserializeObject<T>(val);
            }

            return result;
        }

        #endregion
    }
}
