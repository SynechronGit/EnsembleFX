using EnsembleFX.Messaging.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging
{
    public interface IMessageSubscriber
    {
        /// <summary>
        /// Initializes the specified server context.
        /// </summary>
        /// <param name="serverContext">The server context.</param>
        void Initialize(IServerContext serverContext);

        //void Uninitialize(IServerContext serverContext);

        /// <summary>
        /// Called when [message].
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="session">The session.</param>
        void OnMessage(IMessageEnvelope envelope, int retryCount, ISessionContext session);

        /// <summary>
        /// Gets or sets the execution order.
        /// </summary>
        /// <value>The execution order.</value>
        int ExecutionOrder { get; set; }
    }
}
