using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Serialization
{
    public interface IMessageSerializer : ISerializer
    {
        /// <summary>
        /// Serializes the message.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        string SerializeMessage(IMessage instance);

        /// <summary>
        /// Deserializes the message.
        /// </summary>
        /// <param name="serializedInstance">The serialized instance.</param>
        /// <param name="messageTypeName">Name of the message type.</param>
        /// <returns></returns>
        IMessage DeserializeMessage(string serializedInstance, string messageTypeName);

        /// <summary>
        /// Serializes the envelope.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        string SerializeEnvelope(IMessageEnvelope instance);

        /// <summary>
        /// Deserializes the envelope.
        /// </summary>
        /// <param name="serializedInstance">The serialized instance.</param>
        /// <returns></returns>
        IMessageEnvelope DeserializeEnvelope(string serializedInstance);
    }
}
