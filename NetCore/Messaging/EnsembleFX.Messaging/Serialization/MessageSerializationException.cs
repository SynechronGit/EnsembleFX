using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Serialization
{
    public class MessageSerializationException : System.Exception
    {
             #region Public Properties
        public IMessageEnvelope Envelope { get; set; }
        #endregion

        #region Constructor
        public MessageSerializationException() : base() { }

        public MessageSerializationException(string message) : base(message) { }

        public MessageSerializationException(string message, System.Exception inner) : base(message, inner) { }

        public MessageSerializationException(string message, System.Exception inner, IMessageEnvelope envelope)
            : base(message, inner)
        {
            this.Envelope = envelope;
        }

        public MessageSerializationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        #endregion
    }
}
