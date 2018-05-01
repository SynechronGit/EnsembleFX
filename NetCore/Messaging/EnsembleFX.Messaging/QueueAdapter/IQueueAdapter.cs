using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnsembleFX.Messaging.Service;

namespace EnsembleFX.Messaging.QueueAdapter
{
    /// Delegate for on message
    /// </summary>
    public delegate void OnMessageDelegate(IMessageEnvelope messageEnvelope);

    /// <summary>
    /// Delegate for start receiving
    /// </summary>
    public delegate void StartReceivingDelegate();


    public interface IQueueAdapter : IDisposable
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Initializes the specified server context.
        /// </summary>
        /// <param name="serverContext">The server context.</param>
        void Initialize(IServerContext serverContext);

        /// <summary>
        /// Occurs when [on message].
        /// </summary>
        event OnMessageDelegate OnMessage;

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="messageEnvelope">The message envelope.</param>
        void SendMessage(IMessageEnvelope messageEnvelope);

        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        void Stop();

        /// <summary>
        /// Gets a value indicating whether this instance is control queue.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is control queue; otherwise, <c>false</c>.
        /// </value>
        bool IsControlQueue { get; }

        /// <summary>
        /// Gets the name of the queue.
        /// </summary>
        /// <value>The name of the queue.</value>
        string QueueName { get; }
    }
}
