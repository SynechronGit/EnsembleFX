using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EnsembleFX.Messaging.QueueAdapter
{
    public class SqlQueueConfiguration : ConfigurationSection, IQueueConfiguration
    {
        #region Public Properties
        [XmlAttribute]
        public string Name { get; set; }
        public string MessageType { get; set; }
        public string Contract { get; set; }
        public string Queue { get; set; }
        public string ServiceFrom { get; set; }
        public string ServiceTo { get; set; }
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
        [ConfigurationProperty("messageType")]
        public SqlQueue messageType
        {
            get
            {
                return ((SqlQueue)(base["messageType"]));
            }
            set
            { base["message"] = value; }
        }
        [ConfigurationProperty("contract")]
        public SqlQueue contract
        {
            get
            {
                return ((SqlQueue)(base["contract"]));
            }
            set
            { base["contract"] = value; }
        }
        [ConfigurationProperty("queue")]
        public SqlQueue queue
        {
            get
            {
                return ((SqlQueue)(base["queue"]));
            }
            set
            { base["queue"] = value; }
        }
        [ConfigurationProperty("serviceFrom")]
        public SqlQueue serviceFrom
        {
            get
            {
                return ((SqlQueue)(base["serviceFrom"]));
            }
            set
            { base["serviceFrom"] = value; }
        }
        [ConfigurationProperty("serviceTo")]
        public SqlQueue serviceTo
        {
            get
            {
                return ((SqlQueue)(base["serviceTo"]));
            }
            set
            { base["serviceTo"] = value; }
        }
        [ConfigurationProperty("connectionString")]
        public SqlQueue connectionString
        {
            get
            {
                return ((SqlQueue)(base["connectionString"]));
            }
            set
            { base["connectionString"] = value; }
        }
        [ConfigurationProperty("timeoutInSeconds")]
        public SqlQueue timeoutInSeconds
        {
            get
            {
                return ((SqlQueue)(base["timeoutInSeconds"]));
            }
            set
            { base["timeoutInSeconds"] = value; }
        }
        [ConfigurationProperty("isControlQueue")]
        public SqlQueue isControlQueue
        {
            get
            {
                return ((SqlQueue)(base["isControlQueue"]));
            }
            set
            { base["isControlQueue"] = value; }
        }
    }
    public class SqlQueue : ConfigurationElement
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
