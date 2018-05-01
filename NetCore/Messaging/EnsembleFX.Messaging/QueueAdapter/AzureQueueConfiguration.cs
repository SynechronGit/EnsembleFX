using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EnsembleFX.Messaging.QueueAdapter
{
    public class AzureQueueConfiguration : ConfigurationSection, IQueueConfiguration
    {
        #region Public Properties

        [XmlAttribute]
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public int TimeoutInSeconds { get; set; }
        public bool IsControlQueue { get; set; }

        #endregion

        [ConfigurationProperty("name")]
        public string name
        {
            get
            {
                return Name = (string)this["name"];
            }
            set
            {
                this["name"] = Name;
            }
        }

        [ConfigurationProperty("connectionString")]
        public AzureQueue connectionString
        {
            get
            {
                return ((AzureQueue)(base["connectionString"]));
            }
            set
            { base["connectionString"] = value; }
        }

        [ConfigurationProperty("timeoutInSeconds")]
        public AzureQueue timeoutInSeconds
        {
            get
            {
                return ((AzureQueue)(base["timeoutInSeconds"]));
            }
            set
            { base["timeoutInSeconds"] = value; }
        }

    }

    public class AzureQueue : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }
    }
}
