using EnsembleFX.Helper;
using EnsembleFX.Logging;
using EnsembleFX.Messaging.Bus;
using EnsembleFX.Messaging.Configuration;
using EnsembleFX.Messaging.Logging;
using EnsembleFX.Messaging.Logging;
using EnsembleFX.Messaging.QueueAdapter;
using EnsembleFX.Messaging.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.QueueAdapter
{
    public class AzureQueueAdapter : IQueueAdapter
    {

        #region Events

        public event OnMessageDelegate OnMessage;

        #endregion

        #region IQueueManager Members

        readonly AzureQueueConfiguration _configuration;
        readonly AzureQueueHelper _azureQueueHelper;
        bool _isStop = false;
        readonly IBusLogger _logger;
        IAsyncResult _asyncStart;

        public AzureQueueAdapter(IConfigurationFactory configFactory, string configurationName, IBusLogger logger, ILogController logController, IHttpContextAccessor httpContextAccessor)
        {
            this._configuration = configFactory.GetConfiguration<AzureQueueConfiguration>("AzureQueueConfiguration/Queue");
            _azureQueueHelper = new AzureQueueHelper(logController, httpContextAccessor);
            _azureQueueHelper.TopicName = _configuration.name;
            this._logger = logger;
        }

        public AzureQueueAdapter(AzureQueueConfiguration configuration, IBusLogger logger, ILogController logController, IHttpContextAccessor httpContextAccessor)
        {
            this._configuration = configuration;
            _azureQueueHelper = new AzureQueueHelper(logController, httpContextAccessor);
            _azureQueueHelper.TopicName = _configuration.name;
            this._logger = logger;
        }

        public AzureQueueAdapter(IConfigurationFactory configFactory, string configurationName, IBusLogger logger, string topicName, ILogController logController, IHttpContextAccessor httpContextAccessor)
        {
            this._configuration = configFactory.GetConfiguration<AzureQueueConfiguration>("AzureQueueConfiguration/Queue");
            _azureQueueHelper = new AzureQueueHelper(logController, httpContextAccessor);
            _azureQueueHelper.TopicName = topicName;
            this._logger = logger;
        }

        public void Initialize()
        {
            Initialize(null);
        }

        /// <summary>
        /// Initializes the specified server context.
        /// </summary>
        /// <param name="serverContext">The server context.</param>
        public void Initialize(IServerContext serverContext)
        {
            try
            {
                _azureQueueHelper.CreateTopic(_configuration.Name);  // This is the topic Name
                _azureQueueHelper.RegisterSubscriber("DefaultSubscriber");  // Register DefaultSubscriber - it is required by Azure
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            _isStop = false;
            var start = new StartReceivingDelegate(ReceiveMessageAsync);
            _asyncStart = start.BeginInvoke(null, this);
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="messageEnvelope">The message envelope.</param>
        public void SendMessage(IMessageEnvelope messageEnvelope)
        {
            try
            {
                _azureQueueHelper.SendMessage(messageEnvelope);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Recieves the message async.
        /// </summary>
        protected void ReceiveMessageAsync()
        {
            SubscriptionClient subscriptionClient = SubscriptionClient.Create(_azureQueueHelper.TopicName, _azureQueueHelper.SubscriberName, ReceiveMode.PeekLock);
            BrokeredMessage brokeredMessage;
            IMessageEnvelope receivedMessage;

            while (!_isStop)
            {
                brokeredMessage = null;
                receivedMessage = null;

                try
                {
                    // Get envelope from azure dequeue
                    brokeredMessage = subscriptionClient.Receive(TimeSpan.FromSeconds(Convert.ToInt32(_configuration.TimeoutInSeconds)));
                    if (brokeredMessage != null)
                    {
                        string receivedEnvelope = brokeredMessage.GetBody<string>();

                        JsonMessageSerializer jsonMessageSerializer = new JsonMessageSerializer();
                        receivedMessage = jsonMessageSerializer.DeserializeEnvelope(receivedEnvelope);

                        if (receivedMessage != null)
                        {
                            brokeredMessage.Complete();
                            _logger.LogAdapterSuccess(receivedMessage, "Message Received:" + receivedMessage.MessageUID, this.GetType());
                            OnMessage(receivedMessage);
                        }
                    }
                }
                catch (Microsoft.ServiceBus.Messaging.MessagingException e)
                {
                    if (!e.IsTransient)
                    {
                        if (brokeredMessage != null)
                        {
                            brokeredMessage.Abandon();
                        }
                        throw;
                    }
                    else
                    {
                        Thread.Sleep(2000);  // Retry the message after 2 seconds for non-transient Messaging errors
                    }
                }
                catch (System.Exception ex)
                {
                    _logger.LogAdapterFailure(receivedMessage, ex.Message, ex, this.GetType());
                }
                Thread.Sleep(5000);
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (!_isStop)
            {
                _isStop = true;
                if (_asyncStart != null && _asyncStart.AsyncWaitHandle != null)
                    _asyncStart.AsyncWaitHandle.WaitOne();
            }

        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion

        #region IQueueAdapter Members

        public bool IsControlQueue
        {
            get
            {
                return false;
            }
        }

        public string QueueName
        {
            get
            {
                return _configuration.Name;
            }
        }

        #endregion
    }
}
