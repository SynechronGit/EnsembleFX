using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EnsembleFX.Messaging.Configuration
{
    public class ConfigurationFactory : IConfigurationFactory, IConfigurationSectionHandler
    {
        #region Public Methods

        public T CreateConfiguration<T>(string configurationName)
        {
            return DeserializeConfiguration<T>(configurationName);
        }

        public string SerializeMessage(IConfiguration instance)
        {
            var stream = new MemoryStream();
            var xmlSerializer = new XmlSerializer(instance.GetType());
            xmlSerializer.Serialize(stream, instance);
            byte[] serializeBytes = stream.ToArray();
            string serializeString = System.Text.Encoding.ASCII.GetString(serializeBytes);
            return serializeString;
        }

        public T DeserializeConfiguration<T>(string configurationName)
        {
            T configurationInstance = default(T);
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configurationName);
                StreamReader configStream = File.OpenText(filePath);
                var xmlSerializer = new XmlSerializer(typeof(T));
                configurationInstance = (T)xmlSerializer.Deserialize(configStream);
            }
            catch (System.Exception)
            {

            }
            return configurationInstance;
        }

        public T DeserializeConfigurationFromString<T>(string configurationString) 
        {
            var stream = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(configurationString));
            var xmlSerializer = new XmlSerializer(typeof(T));
            T configurationInstance = (T)xmlSerializer.Deserialize(stream);
            return configurationInstance;
        }

        public T GetConfiguration<T>(string configurationSection) 
        {

            return (T)ConfigurationManager.GetSection(configurationSection);
        }

        #region IConfigurationSectionHandler Members

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            return DeserializeConfigurationFromString<IConfiguration>(section.InnerXml);
        }

        #endregion
        #endregion


    }
}
