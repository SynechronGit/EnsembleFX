using EnsembleFX.Logging;
using EnsembleFX.Messaging.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Service
{
    /// <summary>
    /// Delegate to send message to subscriber
    /// </summary>
    public delegate void SendToSubscriberDelegate(IMessageSubscriber subscriber, IMessageEnvelope envelope, ISessionContext session);

    /// <summary>
    /// Subscriber manager class to implement subscriber manager contract
    /// </summary>
    public class SubscriberManager : ISubscriberManager
    {
        ILogController logController;
        public SubscriberManager(ILogController logController)
        {
            this.logController = logController;
        }

        #region Public Members

        const int MaxRetries = 10;
        const int MaxWaitInMinutes = 120;
        readonly IList<IMessageSubscriber> subscribers;
        readonly IBusLogger logger;
        IServerContext serverContext;
        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriberManager"/> class.
        /// </summary>
        /// <param name="subscribers">The subscribers.</param>
        /// <param name="logger">The logger.</param>
        public SubscriberManager(IMessageSubscriber[] subscribers, IBusLogger logger, ILogController logController)
        {
            this.subscribers = subscribers;
            this.logger = logger;
            this.logController = logController;
        }

        /// <summary>
        /// Determines whether [is registered subscriber] [the specified subscriber].
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <returns>
        /// 	<c>true</c> if [is registered subscriber] [the specified subscriber]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRegisteredSubscriber(IMessageSubscriber subscriber)
        {
            return (this.subscribers != null && this.subscribers.Contains(subscriber) == true);
        }

        /// <summary>
        /// Determines whether [is registered subscriber] [the specified subscriber type].
        /// </summary>
        /// <param name="subscriberType">Type of the subscriber.</param>
        /// <returns>
        /// 	<c>true</c> if [is registered subscriber] [the specified subscriber type]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRegisteredSubscriber(string subscriberType)
        {
            return (this.subscribers != null && subscribers.Where(s => s.GetType().FullName.Equals(subscriberType) == true).Any() == true);
        }

        /// <summary>
        /// Registers the subscriber.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        public void RegisterSubscriber(IMessageSubscriber subscriber)
        {
            subscribers.Add(subscriber);
        }

        /// <summary>
        /// Registers the subscriber.
        /// </summary>
        /// <param name="subscriberType">Type of the subscriber.</param>
        public void RegisterSubscriber(string subscriberType)
        {
            IMessageBusBuilder builder = new MessageBusBuilder(this.logController);
            IMessageSubscriber subscriber = builder.BuildSubscriber(subscriberType);
            if (subscriber != null)
            {
                subscriber.Initialize(serverContext);
                subscribers.Add(subscriber);
            }
        }

        /// <summary>
        /// Unregisters the subscriber.
        /// </summary>
        /// <param name="subscriberType">Type of the subscriber.</param>
        public void UnregisterSubscriber(string subscriberType)
        {
            for (int subscriberCounter = subscribers.Count; subscriberCounter == 0; subscriberCounter--)
            {
                if (subscribers[subscriberCounter].GetType().FullName == subscriberType)
                    subscribers.RemoveAt(subscriberCounter);
            }
        }

        /// <summary>
        /// Initializes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Initialize(IServerContext context)
        {
            this.serverContext = context;
            foreach (IMessageSubscriber subscriber in subscribers)
            {
                subscriber.Initialize(context);
            }
          //  logger.LogSubscribeInfo("Initialized SubscriberManager", this.GetType());
        }

        /// <summary>
        /// Called when [message].
        /// </summary>
        /// <param name="envelope">The envelope.</param>
        /// <param name="specificSubscribers">The specific subscribers.</param>
        public void OnMessage(IMessageEnvelope envelope, IList<string> specificSubscribers)
        {
            var session = new SessionContext();
            Type messageType = envelope.Message.GetType();

            foreach (IMessageSubscriber subscriber in subscribers.OrderBy(sub => sub.ExecutionOrder))
            {
                if (specificSubscribers != null && specificSubscribers.Count > 0)
                {
                    if (specificSubscribers.Contains(subscriber.GetType().FullName))
                    {
                        SendToSubscriberDelegate start = SendToSubscriber;
                        if (subscriber.ExecutionOrder > 1)
                        {
                            start.Invoke(subscriber, envelope, session);
                        }
                        else
                        {
                            start.BeginInvoke(subscriber, envelope, session, null, this);
                        }
                    }
                }
                else if (IsSubscriberConsumer(subscriber.GetType(), messageType))
                {
                    SendToSubscriberDelegate start = SendToSubscriber;
                    if (subscriber.ExecutionOrder > 1)
                    {
                        start.Invoke(subscriber, envelope, session);
                    }
                    else
                    {
                        start.BeginInvoke(subscriber, envelope, session, null, this);
                    }
                }
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (IMessageSubscriber subscriber in subscribers)
            {
                if (subscriber is IDisposable)
                    (subscriber as IDisposable).Dispose();
            }
        }

        #endregion
        #endregion

        #region Protected Methods

        /// <summary>
        /// Sends to subscriber.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="envelope">The envelope.</param>
        /// <param name="session">The session.</param>
        protected void SendToSubscriber(IMessageSubscriber subscriber, IMessageEnvelope envelope, ISessionContext session)
        {
            bool retry = true;
            int retryCount = 0;
            while (retry)
            {
                retryCount++;
                try
                {
                    InvokeOnMessage(subscriber, envelope, retryCount, session);
                    retry = false;
                    logger.LogSubscribeSuccess(envelope, string.Format(CultureInfo.CurrentCulture, "Subscriber {0} successfully processed message {1} (UID: {2}",
                        subscriber.GetType().FullName, envelope.Message.GetType().FullName, envelope.MessageUID), subscriber.GetType());
                }
                catch (Exception.MessagingException messagingEx)
                {
                    logger.LogSubscribeFailure(envelope, messagingEx.Message, messagingEx, subscriber.GetType());
                    retry = messagingEx.Retry;
                    if (retry)
                    {
                        TimeSpan sleepTime = TimeSpan.FromMinutes(MaxWaitInMinutes);

                        if (messagingEx.RetryInterval.Minutes < sleepTime.Minutes)
                            sleepTime = messagingEx.RetryInterval;

                        Thread.Sleep(sleepTime.Milliseconds);
                    }
                }
                catch (System.Exception exception)
                {
                    logger.LogSubscribeFailure(envelope, exception.Message, exception, subscriber.GetType());
                    retry = false;
                }
                if (retryCount >= MaxRetries)
                    retry = false;
            }
        }


        /// <summary>
        /// Determines whether [is subscriber consumer] [the specified subscriber type].
        /// </summary>
        /// <param name="subscriberType">Type of the subscriber.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <returns>
        /// 	<c>true</c> if [is subscriber consumer] [the specified subscriber type]; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsSubscriberConsumer(Type subscriberType, Type messageType)
        {
            Type[] typeArguments = subscriberType.GetGenericArguments();
            bool isDerivedConsumer = typeArguments.Contains(messageType);
            if (!isDerivedConsumer && subscriberType.BaseType != null)
            {
                isDerivedConsumer = IsSubscriberConsumer(subscriberType.BaseType, messageType);
            }
            return isDerivedConsumer;
        }


        /// <summary>
        /// Invokes the on message.
        /// </summary>
        /// <param name="consumer">The consumer.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="retryCount">The retry count.</param>
        /// <param name="session">The session.</param>
        protected void InvokeOnMessage(object consumer, IMessageEnvelope msg, int retryCount, ISessionContext session)
        {
            try
            {
                Type type = consumer.GetType();
                MethodInfo consume = type.GetMethod("OnMessage", new[] { typeof(MessageEnvelope), typeof(int), typeof(SessionContext) });
                var parameters = new List<object> { msg, retryCount, session };
                consume.Invoke(consumer, parameters.ToArray<object>());
            }
            catch (TargetInvocationException ex)
            {
                throw ex;
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }

        #endregion

    }
}
