using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Service
{
    public interface ISubscriberManager : IDisposable
    {
        /// <summary>
        /// Initializes the specified server context.
        /// </summary>
        /// <param name="serverContext">The server context.</param>
        void Initialize(IServerContext serverContext);

        /// <summary>
        /// Registers the subscriber.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        void RegisterSubscriber(IMessageSubscriber subscriber);

        /// <summary>
        /// Called when [message].
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <param name="specificSubscribers">The specific subscribers.</param>
        void OnMessage(IMessageEnvelope envelope, IList<string> specificSubscribers);

        /// <summary>
        /// Registers the subscriber.
        /// </summary>
        /// <param name="subscriberType">Type of the subscriber.</param>
        void RegisterSubscriber(string subscriberType);

        /// <summary>
        /// Unregisters the subscriber.
        /// </summary>
        /// <param name="subscriberType">Type of the subscriber.</param>
        void UnregisterSubscriber(string subscriberType);
    }
}
