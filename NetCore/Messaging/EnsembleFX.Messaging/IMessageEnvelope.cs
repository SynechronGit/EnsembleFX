using EnsembleFX.Messaging.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging
{
    public interface IMessageEnvelope
    {
        DateTime ExpiresOn { get; set; }
        IMessage Message { get; set; }
        DateTime MessageSentOn { get; set; }
        string MessageString { get; set; }
        string MessageType { get; set; }
        Guid MessageUID { get; set; }
        string Originator { get; set; }
        Guid ReplyOf { get; set; }
        string ReplyTo { get; set; }
        string Topic { get; set; }
        string User { get; set; }
        string Environment { get; set; }
        string AllowedEnvironments { get; set; }
    }
}
