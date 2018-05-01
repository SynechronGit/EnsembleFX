using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public class AzureQueueModel
    {
        public string QueueName { get; set; }

        public long MessageCount { get; set; }

    }
}
