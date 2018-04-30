using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace EnsembleFX.Core.Serialization
{
    /// <summary>
    /// Represents class to support common Serialization functionality.
    /// </summary>
    public class XmlSerializationManager : ISerializationManager
    {

        #region Public Methods

        /// <summary>
        /// Deserializes the string content
        /// </summary>
        /// <typeparam name="T">Deserialized object typeof(T)</typeparam>
        /// <param name="content">string content to deserialize</param>
        /// <returns></returns>
        public T DeserializeObject<T>(string content)
        {
            var xs = new XmlSerializer(typeof(T));
            var memoryStream = new MemoryStream(UTF8StringToByteArray(content));
            var stream = new StreamReader(memoryStream, Encoding.UTF8); // Set Encoding to UTF-8
            return (T)xs.Deserialize(stream);
        }

        /// <summary>
        /// Serializes the instance of T to XML
        /// </summary>
        /// <typeparam name="T">Generic type T</typeparam>
        /// <param name="instance">Type instance to serialize</param>
        /// <returns>Serialized representation of the instance as XML string.</returns>
        public string Serialize<T>(T instance)
        {
            var memoryStream = new MemoryStream();
            var xs = new XmlSerializer(typeof(T));
            var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);  // Set Encoding to UTF-8
            xs.Serialize(xmlTextWriter, instance);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            string xmlString = UTF8ByteArrayToString(memoryStream.ToArray());
            return xmlString;
        }

        /// <summary>
        /// Converts UTF-8 byte array to string representation
        /// </summary>
        /// <param name="characters">Byte array of characters</param>
        /// <returns>String representation of the byte array characters</returns>
        public string UTF8ByteArrayToString(byte[] characters)
        {
            var encoding = UTF8Encoding.UTF8;
            string constructedString = encoding.GetString(characters).Trim();
            return (constructedString);
        }

        /// <summary>
        /// Converts string to UTF-8 byte array representation using UTF-8 encoding
        /// </summary>
        /// <param name="content">String to convert to UTF-8 byte array</param>
        /// <returns>Byte array representation of the content string</returns>
        public byte[] UTF8StringToByteArray(string content)
        {
            var encoding = UTF8Encoding.UTF8;
            return encoding.GetBytes(content);
        }

        #endregion

    }
}
