using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Caching
{
    public interface ICacheAdapter
    {
        object Get(string key);
        void Set(string key, object cacheObject, TimeSpan? timeToLive);
        void Remove(string key);
        void RemoveAll();
    }
}
