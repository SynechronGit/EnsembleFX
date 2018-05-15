using EnsembleFX.Caching.Abstractions;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;

namespace EnsembleFX.Caching.RedisCache
{
    public class RedisCacheProvider : IRedisCacheProvider
    {
        private readonly IConfiguration configuration = null;

        public IConnectionMultiplexer Connection { get; protected set; }

        public RedisCacheProvider(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration), "Cannot be null.");
        }

        public RedisCacheProvider CreateConnection()
        {
            if (Connection == null)
            {
                string redisServer = configuration["Caching:Redis:ConnectionString"];
                if (string.IsNullOrEmpty(redisServer))
                {
                    throw new InvalidOperationException("Missing connection string to use redis cache.");
                }
                var lazyConnection = new Lazy<IConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(redisServer));
                Connection = lazyConnection.Value;
                Connection = ConnectionMultiplexer.Connect(redisServer);
            }
            return this;
        }

        public IDatabase GetCacheDataBase()
        {
            if (Connection == null)
            {
                throw new InvalidOperationException("Connection not initialized.");
            }
            return Connection.GetDatabase();
        }


    }
}
