using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EnsembleFX.Messaging.Serialization
{
    public class XMLMessageSerializer : IMessageSerializer
    {
        #region IMessageSerializer<IMessage> Members

        /// <summary>
        /// Serializes the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public string Serialize(object instance)
        {
            var stream = new MemoryStream();
            var xmlSerializer = new XmlSerializer(instance.GetType());
            xmlSerializer.Serialize(stream, instance);
            byte[] serializeBytes = stream.ToArray();
            string serializeString = System.Text.ASCIIEncoding.UTF8.GetString(serializeBytes);
            return serializeString;
        }

        /// <summary>
        /// Deserializes the specified serialized instance.
        /// </summary>
        /// <param name="serializedInstance">The serialized instance.</param>
        /// <param name="messageTypeName">Name of the message type.</param>
        /// <returns></returns>
        public object Deserialize(string serializedInstance, string messageTypeName)
        {
            var stream = new MemoryStream(System.Text.ASCIIEncoding.UTF8.GetBytes(serializedInstance));
            Type messageType = Type.GetType(messageTypeName, true);
            var xmlSerializer = new XmlSerializer(messageType);
            object taskMessage = xmlSerializer.Deserialize(stream);
            return taskMessage;
        }

        /// <summary>
        /// Serializes the message.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public string SerializeMessage(IMessage instance)
        {
            var stream = new MemoryStream();
            var xmlSerializer = new XmlSerializer(instance.GetType());
            xmlSerializer.Serialize(stream, instance);
            byte[] serializeBytes = stream.ToArray();
            string serializeString = System.Text.ASCIIEncoding.UTF8.GetString(serializeBytes);
            return serializeString;
        }

        /// <summary>
        /// Deserializes the message.
        /// </summary>
        /// <param name="serializedInstance">The serialized instance.</param>
        /// <param name="messageTypeName">Name of the message type.</param>
        /// <returns></returns>
        public IMessage DeserializeMessage(string serializedInstance, string messageTypeName)
        {
            var stream = new MemoryStream(System.Text.ASCIIEncoding.UTF8.GetBytes(serializedInstance));
            Type messageType = Type.GetType(messageTypeName, true);
            var xmlSerializer = new XmlSerializer(messageType);
            var taskMessage = xmlSerializer.Deserialize(stream);
            return (IMessage)taskMessage;
        }

        /// <summary>
        /// Serializes the envelope.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public string SerializeEnvelope(IMessageEnvelope instance)
        {
            try
            {
                instance.MessageType = instance.Message.GetType().AssemblyQualifiedName;
                instance.MessageString = SerializeMessage(instance.Message);
            }
            catch (System.Exception ex)
            {
                throw new MessageSerializationException("Could not serialize Message " + instance.MessageType + " for " + instance.Originator + " in " + instance.MessageUID, ex, instance);
            }
            string serializeString = string.Empty;
            try
            {
                var stream = new MemoryStream();
                var xmlSerializer = new XmlSerializer(typeof(MessageEnvelope));
                xmlSerializer.Serialize(stream, instance);
                byte[] serializeBytes = stream.ToArray();
                serializeString = System.Text.ASCIIEncoding.UTF8.GetString(serializeBytes);
            }
            catch (System.Exception ex)
            {
                throw new MessageSerializationException("Could not serialize Message Envelope with Message " + instance.MessageType + " for " + instance.Originator + " in " + instance.MessageUID, ex, instance);
            }
            return serializeString;
        }

        /// <summary>
        /// Deserializes the envelope.
        /// </summary>
        /// <param name="serializedInstance">The serialized instance.</param>
        /// <returns></returns>
        public IMessageEnvelope DeserializeEnvelope(string serializedInstance)
        {
            MessageEnvelope envelope = null;
            try
            {
                var stream = new MemoryStream(System.Text.ASCIIEncoding.UTF8.GetBytes(serializedInstance));
                var xmlSerializer = new XmlSerializer(typeof(MessageEnvelope));
                envelope = (MessageEnvelope)xmlSerializer.Deserialize(stream);
            }
            catch (System.Exception ex)
            {
                throw new MessageSerializationException("Could not Deserialize Message Envelope Containing " + serializedInstance, ex, null);
            }
            try
            {
                envelope.Message = DeserializeMessage(envelope.MessageString, envelope.MessageType);
            }
            catch (System.Exception ex)
            {
                throw new MessageSerializationException("Could not Deserialize Message Envelope with Message " + envelope.MessageType + " for " + envelope.Originator + " in " + envelope.MessageUID, ex, envelope);
            }
            return envelope;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deserializes the specified location URL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="locationURL">The location URL.</param>
        /// <returns></returns>
        public T Deserialize<T>(Uri locationURL)
        {
            MemoryStream stream = null;
            if (locationURL.Scheme.ToLowerInvariant() == "file")
            {
                StreamReader streamReader = File.OpenText(locationURL.LocalPath);
                stream = new MemoryStream(System.Text.UnicodeEncoding.Unicode.GetBytes(streamReader.ReadToEnd()));

            }
            if (locationURL.Scheme.ToLowerInvariant() == "http")
            {
                var httpClient = new System.Net.WebClient();
                stream = new MemoryStream(httpClient.DownloadData(locationURL));

            }
            if (typeof(T).Equals(typeof(string)))
            {
                return (T)Convert.ChangeType(System.Text.UnicodeEncoding.ASCII.GetString(stream.ToArray()), typeof(T), CultureInfo.CurrentCulture);
            }
            else
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                T configurationInstance = (T)xmlSerializer.Deserialize(stream);
                return configurationInstance;
            }
        }

        /// <summary>
        /// Deserializes the specified serialized object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedObject">The serialized object.</param>
        /// <returns></returns>
        public T Deserialize<T>(string serializedObject)
        {
            var stream = new MemoryStream(System.Text.ASCIIEncoding.UTF8.GetBytes(serializedObject));
            return Deserialize<T>(stream);
        }

        /// <summary>
        /// Deserializes the specified serialized object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializedObject">The serialized object.</param>
        /// <returns></returns>
        public T Deserialize<T>(Stream serializedObject)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            T configurationInstance = (T)xmlSerializer.Deserialize(serializedObject);
            return configurationInstance;
        }

        #endregion
    }
}
