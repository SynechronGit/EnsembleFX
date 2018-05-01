using EnsembleFX.Messaging.QueueAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Service
{
    public interface IMessageBus
    {
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        void Dispose();

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Registers the queue adapter.
        /// </summary>
        /// <param name="queueAdapter">The queue adapter.</param>
        void RegisterQueueAdapter(IQueueAdapter queueAdapter);

        /// <summary>
        /// Registers the subscriber.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        void RegisterSubscriber(IMessageSubscriber subscriber);

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

        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();

        /// <summary>
        /// Stops the specified including command.
        /// </summary>
        /// <param name="includingCommand">if set to <c>true</c> [including command].</param>
        void Stop(bool includingCommand);

        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="messageEnvelope">The message envelope.</param>
        void ProcessMessage(IMessageEnvelope messageEnvelope);

        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="messageEnvelope">The message envelope.</param>
        /// <param name="specificSubscribers">The specific subscribers.</param>
        void ProcessMessage(IMessageEnvelope messageEnvelope, IList<string> specificSubscribers);
    }
}
