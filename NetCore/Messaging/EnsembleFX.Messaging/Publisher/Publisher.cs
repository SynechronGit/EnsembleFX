using EnsembleFX.Logging;
using EnsembleFX.Messaging.Logging;
using EnsembleFX.Messaging.Model;
using EnsembleFX.Messaging.QueueAdapter;
using EnsembleFX.Messaging.Serialization;
using EnsembleFX.Messaging.Service;
using EnsembleFX.Repository.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Publisher
{
    public class Publisher<T> : MessageBusBuilder where T : IMessage
    {
        private ILogController logController;
     

        #region Private Properties
        IQueueAdapter _queueManager;
        #endregion

        #region Public Properties
        public string Originator { get; set; }
        public string ReplyTo { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string UserName { get; set; }
        public IList<string> AllowedEnvironments { get; set; }
        #endregion

        #region Protected Members
        protected IBusLogger logger;
        #endregion

        #region Constructor
        public Publisher(ILogController logController, IOptions<ConnectionStrings> connectionStrings, IOptions<AzureServiceBusConfiguration> serviceBusConfiguration) : base(logController, connectionStrings, serviceBusConfiguration)
        {
            _queueManager = this.BuildDefaultQueueAdapter();
            logger = this.BuildLogger();
            _queueManager.Initialize();
            this.logController = logController;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Publisher&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="queueManager">The queue manager.</param>
        /// <param name="logger">The logger.</param>
        public Publisher(IQueueAdapter queueManager, IBusLogger logger, ILogController logController, IOptions<ConnectionStrings> connectionStrings, IOptions<AzureServiceBusConfiguration> serviceBusConfiguration) : base(logController, connectionStrings, serviceBusConfiguration)
        {
            this.logger = logger;
            this._queueManager = queueManager;
            queueManager.Initialize();
            this.logController = logController;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Logs the pre publish failure.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        public virtual void LogPrePublishFailure(T message, System.Exception exception)
        {
            try
            {
                MessageEnvelope envelope = message == null ? new MessageEnvelope(new NullMessage()) : new MessageEnvelope(message);

                envelope.User = this.UserName;
                envelope.Originator = this.Originator;
                envelope.ReplyTo = this.ReplyTo;
                envelope.ExpiresOn = this.ExpiresOn;
                envelope.MessageUID = Guid.NewGuid();
                envelope.MessageSentOn = DateTime.UtcNow;
                string errorMessage = string.Format(CultureInfo.CurrentCulture, "Failed to publish {0} message", envelope.Message.GetType().FullName);
                logger.LogPublishFailure(envelope, errorMessage, exception, envelope.Message.GetType());
            }
            catch (System.Exception)
            {

            }
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public virtual void SendMessage(T message)
        {
            var envelope = new MessageEnvelope(message);
            SendEnvelope(envelope);
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public virtual void SendMessage(T message, string azureServiceBusTopicName)
        {
            _queueManager = BuildAzureQueueAdapterForTopic(azureServiceBusTopicName);

            var envelope = new MessageEnvelope(message);
            SendEnvelope(envelope);
        }

        /// <summary>
        /// Replies the message.
        /// </summary>
        /// <param name="replyOf">The reply of.</param>
        /// <param name="message">The reply message.</param>
        public void ReplyMessage(Guid replyOf, T message)
        {
            var envelope = new MessageEnvelope(message) { ReplyOf = replyOf };
            SendEnvelope(envelope);
        }

        /// <summary>
        /// Sends the envelope.
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        protected void SendEnvelope(IMessageEnvelope envelope)
        {
            try
            {
                envelope.User = this.UserName;
                envelope.Originator = this.Originator;
                envelope.ReplyTo = this.ReplyTo;
                envelope.ExpiresOn = this.ExpiresOn;
                envelope.MessageUID = Guid.NewGuid();
                envelope.MessageSentOn = DateTime.UtcNow;
                _queueManager.SendMessage(envelope);
                logger.LogPublish(envelope, string.Empty, envelope.Message.GetType());
            }
            catch (System.Exception exception)
            {
                logger.LogPublishFailure(envelope, string.Empty, exception, envelope.Message.GetType());
                throw;
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="Publisher&lt;T&gt;"/> is reclaimed by garbage collection.
        /// </summary>
        ~Publisher()
        {
            _queueManager.Stop();
        }
        #endregion

    }
}
