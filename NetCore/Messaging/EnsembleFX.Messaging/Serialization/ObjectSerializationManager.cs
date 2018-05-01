using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace EnsembleFX.Messaging.Serialization
{
    public class ObjectSerializationManager
    {
        #region Private Methods

        /// <summary>
        /// To convert a Byte Array of Unicode values to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        private static string UnicodeByteArrayToString(byte[] characters)
        {
            var encoding = new UnicodeEncoding();
            string constructedString = encoding.GetString(characters).Trim();
            return (constructedString);
        }

        /// <summary>
        /// Converts the String to Unicode Byte array and is used in De serialization
        /// </summary>
        /// <param name="xmlString">The XML string.</param>
        /// <returns></returns>
        private static Byte[] StringToUnicodeByteArray(string xmlString)
        {
            var encoding = new UnicodeEncoding();
            byte[] byteArray = encoding.GetBytes(xmlString);
            return byteArray;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Serialize an object into an XML string
        /// </summary>
        /// <typeparam name="T">Any object type</typeparam>
        /// <param name="obj">The obj.</param>
        /// <returns>xml in string format</returns>
        public string SerializeObject<T>(T obj)
        {
            var memoryStream = new MemoryStream();
            var xs = new XmlSerializer(typeof(T));
            var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.Unicode);
            xs.Serialize(xmlTextWriter, obj);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            string xmlString = UnicodeByteArrayToString(memoryStream.ToArray());
            return xmlString;

        }

        /// <summary>
        /// Reconstruct an object from an XML string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">The XML.</param>
        /// <returns>specified object.</returns>
        public T DeserializeObject<T>(string xml)
        {
            var xs = new XmlSerializer(typeof(T));
            var memoryStream = new MemoryStream(StringToUnicodeByteArray(xml));
            var stream = new StreamReader(memoryStream, Encoding.Unicode);
            return (T)xs.Deserialize(stream);
        }

        #endregion
    }
}
