using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.BuildingBlocks.AzureServiceBus
{
    /// <summary>
    /// Implements Publish to ServiceBus 
    /// </summary>
    public class EventBus : IEventBus
    {
        #region Private Members
        private const string INTEGRATION_EVENT_SUFIX = "IntegrationEvent";
        private IEventBusConnection serviceBus;
        #endregion

        /// <summary>
        /// Initializes instance of ServiceBus from the connection string
        /// </summary>
        /// <param name="serviceBusPersisterConnection">AzureServiceBus connection string</param>
        #region Constructor
        public EventBus(IEventBusConnection serviceBus)
        {
            this.serviceBus = serviceBus;
        }
        #endregion

        #region IEventBus Implementation
        /// <summary>
        /// Creates a Topic Client and Publishes IntegrationEvent to the AzureServiceBus
        /// </summary>
        /// <param name="event">Message sent from the component</param>
        public async Task PublishAsync(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name.Replace(INTEGRATION_EVENT_SUFIX, "");
            var jsonMessage = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var message = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = body,
                Label = eventName,
            };

            var topicClient = serviceBus.CreateModel();

            await topicClient.SendAsync(message);
        }
        #endregion
    }
}
