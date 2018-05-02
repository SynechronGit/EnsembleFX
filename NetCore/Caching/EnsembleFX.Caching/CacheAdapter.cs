using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Caching
{
    public class CacheAdapter : ICacheAdapter
    {
        public object Get(string key)
        {
            throw new NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }

        public void Set(string key, object cacheObject, TimeSpan? timeToLive)
        {
            throw new NotImplementedException();
        }
    }
}
