using EnsembleFX.Messaging.Serialization;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Bus
{
    class JsonMessageSerializer : IMessageSerializer
    {
        public string Serialize(object instance)
        {
            return JsonSerializer.SerializeToString(instance, instance.GetType());
        }

        public string SerializeMessage(IMessage instance)
        {
            return JsonSerializer.SerializeToString(instance, instance.GetType());
        }

        public string SerializeEnvelope(IMessageEnvelope instance)
        {
            try
            {
                instance.MessageType = instance.Message.GetType().AssemblyQualifiedName;
                instance.MessageString = SerializeMessage(instance.Message);
            }
            catch (System.Exception ex)
            {
                throw new Exception.SerializationException("Could not serialize Message " + instance.MessageType + " for " + instance.Originator + " in " + instance.MessageUID, ex, instance);
            }
            string serializeString = string.Empty;
            try
            {
                serializeString = JsonSerializer.SerializeToString(instance, typeof(MessageEnvelope));
            }
            catch (System.Exception ex)
            {
                throw new Exception.SerializationException("Could not serialize Message Envelope with Message " + instance.MessageType + " for " + instance.Originator + " in " + instance.MessageUID, ex, instance);
            }
            return serializeString;
        }

        public IMessageEnvelope DeserializeEnvelope(string serializedInstance)
        {

            MessageEnvelope envelope = null;
            try
            {
                envelope = (MessageEnvelope)JsonSerializer.DeserializeFromString(serializedInstance, typeof(MessageEnvelope));
            }
            catch (System.Exception ex)
            {
                throw new Exception.SerializationException("Could not Deserialize Message Envelope Containing " + serializedInstance, ex, null);
            }
            try
            {
                envelope.Message = DeserializeMessage(envelope.MessageString, envelope.MessageType);
            }
            catch (System.Exception ex)
            {
                throw new Exception.SerializationException("Could not Deserialize Message Envelope with Message " + envelope.MessageType + " for " + envelope.Originator + " in " + envelope.MessageUID, ex, envelope);
            }
            return envelope;
        }

        public IMessage DeserializeMessage(string serializedInstance, string messageTypeName)
        {
            Type messageType = Type.GetType(messageTypeName, true);
            var deserializedMessage = (IMessage)JsonSerializer.DeserializeFromString(serializedInstance, messageType);
            return deserializedMessage;
        }

        public object Deserialize(string serializedInstance, string messageTypeName)
        {
            Type messageType = Type.GetType(messageTypeName, true);
            object deserializedMessage = JsonSerializer.DeserializeFromString(serializedInstance, messageType);
            return deserializedMessage;
        }

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
                return JsonSerializer.DeserializeFromStream<T>(stream);
            }
        }

        public T Deserialize<T>(string serializedObject)
        {
            return JsonSerializer.DeserializeFromString<T>(serializedObject);
        }

        public T Deserialize<T>(Stream serializedObject)
        {
            return JsonSerializer.DeserializeFromStream<T>(serializedObject);
        }
    }
}
