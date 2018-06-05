using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public class AzureServiceBusConfiguration
    {
        public string ClientId { get; set; }
        public string ClientSecrete { get; set; }
        public string TenantId { get; set; }
        public string ResourceGroupName { get; set; }
    }
}
