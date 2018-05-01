using EnsembleFX.Messaging.Logging;
using EnsembleFX.Messaging.QueueAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Service
{
    #region Public Members;
    public delegate void OnMessageAsync(IMessageEnvelope messageEnvelope, IList<string> specificSubscribers);
    #endregion

    /// <summary>
    /// Represents the Message Bus to carry out asyn operations
    /// </summary>
    public class MessageBus : IDisposable, IMessageBus
    {
        #region Private Members
        readonly IList<IQueueAdapter> _queueManagers;
        readonly ISubscriberManager _subscriberManager;
        IServerContext _serverContext;
        readonly IBusLogger _logger;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageBus"/> class.
        /// </summary>
        /// <param name="queueManagers">The queue managers.</param>
        /// <param name="subscriberManager">The subscriber manager.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="interpreter">The interpreter.</param>
        public MessageBus(IQueueAdapter[] queueManagers, ISubscriberManager subscriberManager, IBusLogger logger)
        {
            this._queueManagers = queueManagers;
            this._subscriberManager = subscriberManager;
            _serverContext = new ServerContext { ServerName = Environment.MachineName };
            this._logger = logger;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Registers the queue adapter.
        /// </summary>
        /// <param name="queueAdapter">The queue adapter.</param>
        public void RegisterQueueAdapter(IQueueAdapter queueAdapter)
        {
            _queueManagers.Add(queueAdapter);
        }

        /// <summary>
        /// Registers the subscriber.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        public void RegisterSubscriber(IMessageSubscriber subscriber)
        {
            this._subscriberManager.RegisterSubscriber(subscriber);
        }

        /// <summary>
        /// Registers the subscriber.
        /// </summary>
        /// <param name="subscriberType">Type of the subscriber.</param>
        public void RegisterSubscriber(string subscriberType)
        {
            this._subscriberManager.RegisterSubscriber(subscriberType);
        }

        /// <summary>
        /// Unregisters the subscriber.
        /// </summary>
        /// <param name="subscriberType">Type of the subscriber.</param>
        public void UnregisterSubscriber(string subscriberType)
        {
            this._subscriberManager.UnregisterSubscriber(subscriberType);
        }

        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="messageEnvelope">The message envelope.</param>
        public void ProcessMessage(IMessageEnvelope messageEnvelope)
        {
            OnMessage(messageEnvelope, null);
        }

        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="messageEnvelope">The message envelope.</param>
        /// <param name="specificSubscribers">The specific subscribers.</param>
        public void ProcessMessage(IMessageEnvelope messageEnvelope, IList<string> specificSubscribers)
        {
            OnMessage(messageEnvelope, specificSubscribers);
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="messageEnvelope">The message envelope.</param>
        public void SendMessage(string queueName, IMessageEnvelope messageEnvelope)
        {
            foreach (IQueueAdapter queueManager in _queueManagers)
            {
                if (queueManager.QueueName == queueName)
                {
                    queueManager.SendMessage(messageEnvelope);
                }
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            //_logger.LogBusInfo("Initializing Message Bus :" + this.GetType(), this.GetType());
            _serverContext = new ServerContext();
            _serverContext.Clear();
            foreach (IQueueAdapter queueManager in _queueManagers)
            {
                queueManager.Initialize(_serverContext);
                queueManager.OnMessage += new OnMessageDelegate(queueManager_OnMessage);
            }
            this._subscriberManager.Initialize(_serverContext);
            //_logger.LogBusInfo("Initialized Message Bus :" + this.GetType(), this.GetType());
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            foreach (IQueueAdapter queueManager in _queueManagers)
            {
                queueManager.Start();
            }
            //_logger.LogBusInfo("Started Message Bus :" + this.GetType(), this.GetType());
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            Stop(false);
        }

        /// <summary>
        /// Stops the specified including command.
        /// </summary>
        /// <param name="includingCommand">if set to <c>true</c> [including command].</param>
        public void Stop(bool includingCommand)
        {
            foreach (IQueueAdapter queueManager in _queueManagers)
            {
                if (queueManager.IsControlQueue)
                {
                    if (includingCommand)
                        queueManager.Stop();
                }
                else
                {
                    queueManager.Stop();
                }
            }
            _logger.LogBusInfo("Stopped Message Bus :" + this.GetType(), this.GetType());
        }


        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _serverContext.Clear();
            this._subscriberManager.Dispose();
        }

        #endregion
        #endregion

        #region Private Methods

        /// <summary>
        /// Queues the manager_ on message.
        /// </summary>
        /// <param name="messageEnvelope">The message envelope.</param>
        void queueManager_OnMessage(IMessageEnvelope messageEnvelope)
        {
            OnMessage(messageEnvelope, null);
        }

        /// <summary>
        /// Called when [message].
        /// </summary>
        /// <param name="messageEnvelope">The message envelope.</param>
        /// <param name="specificSubscribers">The specific subscribers.</param>
        void OnMessage(IMessageEnvelope messageEnvelope, IList<string> specificSubscribers)
        {
            _logger.LogBusReceived(messageEnvelope, "");
            try
            {
                var session = new SessionContext();
                var OnMessageCall = new OnMessageAsync(_subscriberManager.OnMessage);
                OnMessageCall.BeginInvoke(messageEnvelope, specificSubscribers, null, this);

            }
            catch (System.Exception exception)
            {
                _logger.LogBusReceivedFailure(messageEnvelope, exception.Message, exception);
            }
        }

        #endregion

    }


}
