using EnsembleFX.Messaging.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging
{
    public interface IQueueConfiguration : IConfiguration
    {
        bool IsControlQueue { get; set; }
    }
}
