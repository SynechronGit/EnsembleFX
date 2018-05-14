using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Core.Helpers
{
    public static class HashSetExtensions
    {
        public static HashSet<string> Add(this HashSet<string> hashSet, string prefix, IEnumerable<string> itemsToAdd)
        {
            foreach (var item in itemsToAdd)
            {
                hashSet.Add(prefix + ":" + item.Replace(" ", "_"));
            }

            return hashSet;
        }
    }
}
