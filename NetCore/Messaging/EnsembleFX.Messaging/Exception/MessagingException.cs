using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Exception
{
    public class MessagingException : System.Exception
    {
        #region Public Properties
        public bool Retry { get; set; }

        public TimeSpan RetryInterval { get; set; }

        public Guid MessageID { get; set; }

        public bool IsFatal { get; set; }
        #endregion

        #region Constructor
        public MessagingException() : base() { }

        public MessagingException(string message) : base(message) { }

        public MessagingException(string message, System.Exception inner) : base(message, inner) { }

        public MessagingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        #endregion
    }
}
