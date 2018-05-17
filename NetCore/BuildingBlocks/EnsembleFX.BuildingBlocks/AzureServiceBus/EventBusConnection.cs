using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;

namespace EnsembleFX.BuildingBlocks.AzureServiceBus
{
    /// <summary>
    /// Creates a persistent connection to AzureServiceBus and creates a Topic Client
    /// </summary>
    public class EventBusConnection : IEventBusConnection
    {
        #region Private Members
        private readonly IConfiguration configuration = null;
        private ServiceBusConnectionStringBuilder _serviceBusConnectionStringBuilder;
        private ITopicClient topicClient;

        bool disposed;
        #endregion

        #region Constructor
        /// <summary>
        /// Establishes AzureServiceBus connection by getting the connection parameters through Configuration Object
        /// </summary>
        /// <param name="configuration"></param>
        public EventBusConnection(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration), "Cannont be null.");
        }
        #endregion

        #region IServiceBusPersisterConnection Implementation
        /// <summary>
        /// Constructs a connection string for creating client messaging entities
        /// </summary>
        public ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder => _serviceBusConnectionStringBuilder;

        /// <summary>
        /// Creates a TopicClient which is used for all basic interactions with a Service Bus topic.
        /// </summary>
        /// <returns>Instance of TopicClient</returns>
        public ITopicClient CreateModel()
        {
            EnsureConnection();
            if (topicClient == null || topicClient.IsClosedOrClosing)
            {
                topicClient = new TopicClient(_serviceBusConnectionStringBuilder, RetryPolicy.Default);
            }
            return topicClient;
        }
        #endregion

        //Method for building the Connection string 
        private void EnsureConnection()
        {
            var connectionString = configuration["AzureServiceBus:Endpoint"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Missing Endpoint settings in AzureServiceBus section.");
            }
            var entityPath = configuration["AzureServiceBus:EntityPath"];
            if (string.IsNullOrEmpty(entityPath))
            {
                throw new InvalidOperationException("Missing EntityPath settings in AzureServiceBus section.");
            }
            var sharedAccessKeyName = configuration["AzureServiceBus:SharedAccessKeyName"];
            if (string.IsNullOrEmpty(sharedAccessKeyName))
            {
                throw new InvalidOperationException("Missing SharedAccessKeyName settings in AzureServiceBus section.");
            }
            var sharedAccessKey = configuration["AzureServiceBus:SharedAccessKey"];
            if (string.IsNullOrEmpty(sharedAccessKey))
            {
                throw new InvalidOperationException("Missing SharedAccessKey settings in AzureServiceBus section.");
            }

            _serviceBusConnectionStringBuilder = new ServiceBusConnectionStringBuilder(connectionString, entityPath, sharedAccessKeyName, sharedAccessKey);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (disposed) return;

            disposed = true;
        }
    }
}
