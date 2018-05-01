using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Exception
{
    public class SerializationException : System.Exception
    {
        #region Public Properties
        public IMessageEnvelope Envelope { get; set; }
        #endregion

        #region Constructor
        public SerializationException() : base() { }

        public SerializationException(string message) : base(message) { }

        public SerializationException(string message, System.Exception inner) : base(message, inner) { }

        public SerializationException(string message, System.Exception inner, IMessageEnvelope envelope)
            : base(message, inner)
        {
            this.Envelope = envelope;
        }

        public SerializationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        #endregion
    }
}
