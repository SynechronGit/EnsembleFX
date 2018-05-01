using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace EnsembleFX.Serialization
{
    public class XmlHelper
    {
        #region Public Methods
        /// <summary>
        /// It is DataContractSerializer.
        /// </summary>
        /// <typeparam name="T">Class Type</typeparam>
        /// <param name="data">Data to be serialized</param>
        /// <returns>XML representation of serialized data</returns>
        public static string DCSerializer<T>(T data)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            string xmlString;

            using (var sw = new StringWriter())
            {
                using (var writer = new XmlTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;
                    serializer.WriteObject(writer, data);
                    writer.Flush();
                    xmlString = sw.ToString();
                }
            }

            return xmlString.Replace("  ", "").Replace(@"\n", "").Replace(@"\r", "").Replace("@\t", "");
        }

        public static string SerializeObject<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
            StringWriter textWriter = new StringWriter();

            xmlSerializer.Serialize(textWriter, toSerialize);
            return textWriter.ToString();
        }

        public static string SerializeObject(Type input, object toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(input);
            StringWriter textWriter = new StringWriter();

            xmlSerializer.Serialize(textWriter, toSerialize);
            return textWriter.ToString();
        }

        public static T DeserializeObject<T>(string xml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            T result;
            using (TextReader reader = new StringReader(xml))
            {
                result = (T)xmlSerializer.Deserialize(reader);
            }

            return result;
        }

        public static object DeserializeObject(string xml, Type input)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(input);
            object result;
            using (TextReader reader = new StringReader(xml))
            {
                result = xmlSerializer.Deserialize(reader);
            }
            return result;
        }
        #endregion

    }
}
