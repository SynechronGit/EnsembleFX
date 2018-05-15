using EnsembleFX.Caching.RedisCache;
using StackExchange.Redis;


namespace EnsembleFX.Caching.Abstractions
{
    /// <summary>
    /// Redis cache connection provider
    /// </summary>
    public interface IRedisCacheProvider
    {
        /// <summary>
        /// Instance of <see cref="ConnectionMultiplexer"/>
        /// </summary>
        IConnectionMultiplexer Connection { get; }
        /// <summary>
        /// Creates laze connection to redis server
        /// </summary>
        /// <returns></returns>
        RedisCacheProvider CreateConnection();
        /// <summary>
        /// Gets cache database
        /// </summary>
        /// <exception cref="InvalidOperationException">If connection is not created</exception>
        /// <returns>An instance of <see cref="IDatabase"/></returns>
        IDatabase GetCacheDataBase();
    }
}
