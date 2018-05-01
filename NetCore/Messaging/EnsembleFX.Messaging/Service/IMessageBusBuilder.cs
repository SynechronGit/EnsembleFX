using EnsembleFX.Messaging.Configuration;
using EnsembleFX.Messaging.Logging;
using EnsembleFX.Messaging.QueueAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Service
{
    public interface IMessageBusBuilder
    {
        /// <summary>
        /// Builds the message bus.
        /// </summary>
        /// <returns></returns>
        IMessageBus BuildMessageBus();

        /// <summary>
        /// Builds the queue adapters.
        /// </summary>
        /// <returns></returns>
        IList<IQueueAdapter> BuildQueueAdapters();

        /// <summary>
        /// Builds the subscriber manager.
        /// </summary>
        /// <returns></returns>
        ISubscriberManager BuildSubscriberManager();

        /// <summary>
        /// Builds the subscribers.
        /// </summary>
        /// <returns></returns>
        IList<IMessageSubscriber> BuildSubscribers();

        /// <summary>
        /// Builds the command interpreter.
        /// </summary>
        /// <returns></returns>
        // ICommandInterpreter BuildCommandInterpreter();

        /// <summary>
        /// Builds the logger.
        /// </summary>
        /// <returns></returns>
        IBusLogger BuildLogger();

        /// <summary>
        /// Builds the configuration factory.
        /// </summary>
        /// <returns></returns>
        IConfigurationFactory BuildConfigurationFactory();

        /// <summary>
        /// Builds the subscriber.
        /// </summary>
        /// <param name="instanceName">Name of the instance.</param>
        /// <returns></returns>
        IMessageSubscriber BuildSubscriber(string instanceName);

        /// <summary>
        /// Resolves the specified type instance.
        /// </summary>
        /// <param name="typeInstance">The type instance.</param>
        /// <returns></returns>
        object Resolve(Type typeInstance);
    }
}
