using Microsoft.Azure.ServiceBus;
using System;

namespace EnsembleFX.BuildingBlocks.AzureServiceBus
{
    /// <summary>
    /// Interface defining ServiceBus ConnectionString property and method for creating TopicClient
    /// </summary>
    public interface IEventBusConnection : IDisposable
    {
        /// <summary>
        /// AzureServiceBus connection string property
        /// </summary>
        ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }

        /// <summary>
        /// Creates Topic Client for AzureServiceBus using the connection string
        /// </summary>
        ITopicClient CreateModel();
    }
}
