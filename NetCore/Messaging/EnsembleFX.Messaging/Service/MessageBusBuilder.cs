﻿using EnsembleFX.Messaging.Configuration;
using EnsembleFX.Messaging.Logging;
using EnsembleFX.Messaging.QueueAdapter;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Service
{
    public class MessageBusBuilder : IMessageBusBuilder
    {
        #region Public Methods

        /// <summary>
        /// Builds the message bus.
        /// </summary>
        /// <returns></returns>
        public IMessageBus BuildMessageBus()
        {
            IBusLogger logger = BuildLogger();
            IMessageBus bus = new MessageBus(BuildQueueAdapters().ToArray(), BuildSubscriberManager(), logger);
            return bus;
        }

        /// <summary>
        /// Resolves the specified type instance.
        /// </summary>
        /// <param name="typeInstance">The type instance.</param>
        /// <returns></returns>
        public object Resolve(Type typeInstance)
        {
            return Activator.CreateInstance(typeInstance);
        }


        /// <summary>
        /// Builds the subscriber.
        /// </summary>
        /// <param name="instanceName">Name of the instance.</param>
        /// <returns></returns>
        public IMessageSubscriber BuildSubscriber(string instanceName)
        {
            return null;
        }

        /// <summary>
        /// Builds the subscribers.
        /// </summary>
        /// <returns></returns>
        public IList<IMessageSubscriber> BuildSubscribers()
        {
            return null;
        }

        /// <summary>
        /// Builds the subscriber manager.
        /// </summary>
        /// <returns></returns>
        public ISubscriberManager BuildSubscriberManager()
        {
            IBusLogger logger = BuildLogger();
            ISubscriberManager subscriberManager = new SubscriberManager(BuildSubscribers().ToArray(), logger);
            return subscriberManager;
        }

        /// <summary>
        /// Builds the queue adapters.
        /// </summary>
        /// <returns></returns>
        public IList<IQueueAdapter> BuildQueueAdapters()
        {
            IList<IQueueAdapter> queueAdapters = new List<IQueueAdapter>
                                                     {
                                                         BuildDefaultQueueAdapter(),
                                                         BuildCommandQueueAdapter()
                                                     };
            return queueAdapters;
        }

        /// <summary>
        /// Builds the logger.
        /// </summary>
        /// <returns></returns>
        public IBusLogger BuildLogger()
        {
            try
            {
                return ServiceLocator.Current != null ? ServiceLocator.Current.GetInstance<IBusLogger>() : new SqlBusLogger();
            }
            catch (System.Exception exp)
            {
                return new SqlBusLogger();
            }
        }
        /// <summary>
        /// Builds the configuration factory.
        /// </summary>
        /// <returns></returns>
        public IConfigurationFactory BuildConfigurationFactory()
        {
            return new ConfigurationFactory();
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Builds the default queue adapter.
        /// </summary>
        /// <returns></returns>
        protected IQueueAdapter BuildDefaultQueueAdapter()
        {
            //Code break if ServiceLocator.Current is null, so if ServiceLocator.Current is null then create from build Azure Queue Adapter
            try
            {
                return ServiceLocator.Current != null ? ServiceLocator.Current.GetInstance<IQueueAdapter>() : BuildAzureQueueAdapter();
            }
            catch (System.Exception exp)
            {
                return BuildAzureQueueAdapter();
            }
            return null;
        }

        /// <summary>
        /// Builds the command queue adapter.
        /// </summary>
        /// <returns></returns>
        protected IQueueAdapter BuildCommandQueueAdapter()
        {
            IBusLogger logger = new SqlBusLogger();
            return new SqlQueueAdapter(BuildConfigurationFactory(), "Command.Queue.Config.xml", logger);
        }

        protected IQueueAdapter BuildAzureQueueAdapterForTopic(string azureServiceBusTopicName)
        {
            IBusLogger logger = new SqlBusLogger();
            return new AzureQueueAdapter(BuildConfigurationFactory(), "Not_In_Use.xml", logger, azureServiceBusTopicName);
        }

        protected IQueueAdapter BuildAzureQueueAdapter()
        {
            IBusLogger logger = new SqlBusLogger();
            return new AzureQueueAdapter(BuildConfigurationFactory(), "Not_In_Use.xml", logger);
        }

        #endregion
    }
}
