using System.IO;
using System.Runtime.Serialization;
using System.Text;


namespace EnsembleFX.Core.Serialization
{
    /// <summary>
    /// Represents a class used to desserialize and/or serialize a DataContract
    /// </summary>
    public class DataContractSerializationManager : ISerializationManager
    {
        /// <summary>
        /// Deserializes an object.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>Returns a deserialized object of type T.</returns>
        public T DeserializeObject<T>(string content)
        {
            MemoryStream strm = new MemoryStream(UTF8StringToByteArray(content));
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            return (T)serializer.ReadObject(strm);
        }

        /// <summary>
        /// Serializes the specified instance.
        /// </summary>
        /// <param name="instance">The instance to serialize.</param>
        /// <returns>Returns a UTF8 byte array as a string.</returns>
        public string Serialize<T>(T instance)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            MemoryStream strm = new MemoryStream();
            serializer.WriteObject(strm, instance);
            return UTF8ByteArrayToString(strm.ToArray());
        }

        /// <summary>
        /// UTs the f8 byte array to string.
        /// </summary>
        /// <param name="characters">The characters.</param>
        /// <returns></returns>
        public string UTF8ByteArrayToString(byte[] characters)
        {
            var encoding = UTF8Encoding.UTF8;  
            string constructedString = encoding.GetString(characters).Trim();
            return (constructedString);
        }

        /// <summary>
        /// Gets the bytes of a UTF8 string and returns a byte array.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>A byte array of the specified string.</returns>
        public byte[] UTF8StringToByteArray(string content)
        {
            var encoding = UTF8Encoding.UTF8;
            return encoding.GetBytes(content);
        }
    }
}
