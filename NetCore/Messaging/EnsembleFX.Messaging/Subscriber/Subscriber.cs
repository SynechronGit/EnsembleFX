using EnsembleFX.Messaging.Serialization;
using EnsembleFX.Messaging.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Subscriber
{
    public class Subscriber<T> : ISubscriber<T> where T : IMessage
    {

        #region Public Methods
        #region IMessageConsumer Members

        /// <summary>
        /// Initializes the specified server context.
        /// </summary>
        /// <param name="serverContext">The server context.</param>
        public virtual void Initialize(IServerContext serverContext)
        {
            return;
        }

        /// <summary>
        /// Called when [message].
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="session">The session.</param>
        public virtual void OnMessage(IMessageEnvelope envelope, int retryCount, ISessionContext session)
        {

        }

        #endregion
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the execution order.
        /// </summary>
        /// <value>The execution order.</value>
        public int ExecutionOrder
        { get; set; }
        #endregion
    }
}
