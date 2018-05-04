using EnsembleFX.Logging;
using EnsembleFX.Messaging.Logging;
using EnsembleFX.Messaging.QueueAdapter;
using EnsembleFX.Messaging.Serialization;
using EnsembleFX.Messaging.Service;
using EnsembleFX.Repository.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Publisher
{
    public class PublisherBuilder<T> : MessageBusBuilder, IPublisherBuilder<T> where T : IMessage
    {
        ILogController logController;
        IOptions<ConnectionStrings> connectionStrings;
        public PublisherBuilder(ILogController logController, IOptions<ConnectionStrings> connectionStrings) : base(logController, connectionStrings)
        {
            this.logController = logController;
            this.connectionStrings = connectionStrings;
        }

        /// <summary>
        /// Builds the publisher.
        /// </summary>
        /// <param name="queueAdapter">The queue adapter.</param>
        /// <returns></returns>
        public Publisher<T> BuildPublisher(IQueueAdapter queueAdapter)
        {
            if (queueAdapter == null)
                queueAdapter = BuildDefaultQueueAdapter();
            IBusLogger logger = BuildLogger();
            var publisher = new Publisher<T>(queueAdapter, logger, this.logController, this.connectionStrings);
            return publisher;
        }

        /// <summary>
        /// Builds the publisher.
        /// </summary>
        /// <returns></returns>
        public Publisher<T> BuildPublisher()
        {
            IQueueAdapter queueAdapter = BuildDefaultQueueAdapter();
            IBusLogger logger = BuildLogger();
            var publisher = new Publisher<T>(queueAdapter, logger, this.logController, this.connectionStrings);
            return publisher;
        }


        #region IPublisherBuilder<T> Members

        IMessagePublisher<T> IPublisherBuilder<T>.BuildPublisher(IQueueAdapter queueAdapter)
        {
            throw new NotImplementedException();
        }

        IMessagePublisher<T> IPublisherBuilder<T>.BuildPublisher()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
