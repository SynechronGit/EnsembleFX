using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Core.DataFrame.Adapters
{
    public class AzureTableAdapterConfiguration : IAdapterConfiguration
    {
        public string Name { get; set; }

        public string StorageConnectionString { get; set; }

        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

    }
}
