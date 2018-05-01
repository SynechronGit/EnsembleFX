using EnsembleFX.Messaging.Configuration;
using EnsembleFX.Messaging.Logging;
using EnsembleFX.Messaging.QueueAdapter;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Service
{
    public class UnityMessageBusBuilder : IMessageBusBuilder, IDisposable
    {
        #region Private Members
        readonly IUnityContainer _container;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityMessageBusBuilder"/> class.
        /// </summary>
        /// <param name="unityConfigurationPath">The unity configuration path.</param>
        public UnityMessageBusBuilder(string unityConfigurationPath)
        {
            //Unity configuration
            this._container = new UnityContainer();

            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Configure(_container, "default");

            _container.RegisterTypes(
                      AllClasses.FromLoadedAssemblies().Where(t => !string.IsNullOrEmpty(t.Namespace) &&
                          (t.Namespace.StartsWith("EnsembleFX") || t.Namespace.StartsWith("EnsembleFX.Runtime.Api") || t.Namespace.StartsWith("Ensemble.")) && (t.Name.EndsWith("Manager") || t.Name.EndsWith("Repository"))),
                      WithMappings.FromMatchingInterface,
                      WithName.Default,
                      WithLifetime.Transient);

            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(_container));
            
        }
        #endregion

        #region Public Methods

        #region IMessageBusBuilder Members

        /// <summary>
        /// Builds the message bus.
        /// </summary>
        /// <returns></returns>
        public IMessageBus BuildMessageBus()
        {
            return _container.Resolve<IMessageBus>();
        }

        /// <summary>
        /// Resolves the specified type instance.
        /// </summary>
        /// <param name="typeInstance">The type instance.</param>
        /// <returns></returns>
        public object Resolve(Type typeInstance)
        {
            return _container.Resolve(typeInstance);
        }

        /// <summary>
        /// Builds the queue adapters.
        /// </summary>
        /// <returns></returns>
        public IList<IQueueAdapter> BuildQueueAdapters()
        {
            return new List<IQueueAdapter>(_container.ResolveAll<IQueueAdapter>());
        }

        /// <summary>
        /// Builds the subscriber manager.
        /// </summary>
        /// <returns></returns>
        public ISubscriberManager BuildSubscriberManager()
        {
            return _container.Resolve<ISubscriberManager>();
        }

        /// <summary>
        /// Builds the subscribers.
        /// </summary>
        /// <returns></returns>
        public IList<IMessageSubscriber> BuildSubscribers()
        {
            return new List<IMessageSubscriber>(_container.ResolveAll<IMessageSubscriber>());
        }

        /// <summary>
        /// Builds the subscriber.
        /// </summary>
        /// <param name="instanceName">Name of the instance.</param>
        /// <returns></returns>
        public IMessageSubscriber BuildSubscriber(string instanceName)
        {
            return _container.Resolve<IMessageSubscriber>(instanceName);
        }

        /// <summary>
        /// Builds the logger.
        /// </summary>
        /// <returns></returns>
        public IBusLogger BuildLogger()
        {
            return _container.Resolve<IBusLogger>();
        }

        /// <summary>
        /// Builds the configuration factory.
        /// </summary>
        /// <returns></returns>
        public IConfigurationFactory BuildConfigurationFactory()
        {
            return _container.Resolve<IConfigurationFactory>();
        }

        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(true);
        }
        #endregion

        #region Private Methods

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed.
        private void Dispose(bool disposing)
        {
            // If disposing equals true, dispose all managed 
            // and unmanaged resources.
            if (disposing)
            {
                // Dispose managed resources.
                if (_container != null)
                {
                    _container.Dispose();
                }
            }
        }
        #endregion
    }

}
