using EnsembleFX.Messaging.QueueAdapter;
using EnsembleFX.Messaging.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Publisher
{
    public interface IPublisherBuilder<T>
      where T : IMessage
    {
        IMessagePublisher<T> BuildPublisher(IQueueAdapter queueAdapter);
        IMessagePublisher<T> BuildPublisher();
    }
}
