using EnsembleFX.Messaging.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Service
{
    public interface ISubscriber<T> : IMessageSubscriber where T : IMessage
    {

    }
}
