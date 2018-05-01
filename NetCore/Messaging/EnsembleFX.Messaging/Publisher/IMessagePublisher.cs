using EnsembleFX.Messaging.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Publisher
{
    public interface IMessagePublisher<T> where T : IMessage
    {
        DateTime ExpiresOn { get; set; }
        void LogPrePublishFailure(object message, System.Exception exception);
        string Originator { get; set; }
        void ReplyMessage(Guid replyOf, object replyMessage);
        string ReplyTo { get; set; }
        void SendMessage(object message);
        string UserName { get; set; }
    }
}
