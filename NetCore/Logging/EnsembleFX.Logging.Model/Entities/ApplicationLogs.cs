using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnsembleFX.Logging.Enums;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using EnsembleFX.Core.Security;
using Microsoft.WindowsAzure.Storage.Table;

namespace EnsembleFX.Logging.Entities
{
    [Serializable]
    [DataContract]
    public class ApplicationLogs : TableEntity
    {
        #region Constructors

        public ApplicationLogs()
        {
            Timestamp = DateTimeOffset.Now;
            PartitionKey = DateTime.Now.Year.ToString();
            RowKey = Guid.NewGuid().ToString();
        }

        public ApplicationLogs(string partitionKey, string rowKey)
        {
            Timestamp = DateTimeOffset.Now;
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        #endregion

        #region Properties

        ///// <summary>
        ///// The logging level, which defaults to <see cref="Ensemble.Framework.Logging.LogLevel.Info"/>.
        ///// </summary>
        [DataMember]
        public LogLevel LogLevel { get; set; }

        ///// <summary>
        ///// A summarizing title for the logged entry. Defaults to
        ///// <c>String.Empty</c>.
        ///// </summary>
        //[DataMember]
        //public string Title { get; set; }

        /// <summary>
        /// The logged message body. Defaults to
        /// <c>String.Empty</c>.
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// The name of the logger used to log this item.
        /// </summary>
        [DataMember]
        public string LoggerName { get; set; }

        /// <summary>
        /// Allows to attach an exception to the message.
        /// Defaults to <c>null</c>.
        /// </summary>
        [XmlIgnore]
        [DataMember]
        public Exception Exception { get; set; }

        /// <summary>
        /// Event number or identifier. Defaults to null.
        /// </summary>
        [DataMember]
        public int? EventId { get; set; }

        /// <summary>
        /// Additional properties that extend the functionality of what can be logged
        /// for use by custom loggers
        /// </summary>
        [DataMember]
        public EnterpriseIdentity EnterpriseIdentity
        {
            get;
            set;
        }

        /// <summary>
        /// Key Value based Dictionary object to pass on arbitrary values to Loggers
        /// </summary>
        [DataMember]
        public Dictionary<string, object> ExtensionElements
        {
            get;
            set;
        }

        /// <summary>
        /// Exception stack trace 
        /// </summary>
        [DataMember]
        public string StackTrace
        {
            get;
            set;
        }

        /// <summary>
        /// Log Level Enum Type Value
        /// </summary>
        [DataMember]
        public string LogLevelType { get; set; }

        [DataMember]
        public string Thread { get; set; }

        [DataMember]
        public string Environment { get; set; }

        [DataMember]
        public string EventName { get; set; }

        [DataMember]
        public string UrlReferrer { get; set; }

        [DataMember]
        public string ClientBrowser { get; set; }

        [DataMember]
        public string ClientIP { get; set; }

        [DataMember]
        public string URL { get; set; }

        [DataMember]
        public string ApplicationIdentifier { get; set; }

        [DataMember]
        public string Source { get; set; }

        [DataMember]
        public string RequestObject { get; set; }

        [DataMember]
        public string ResponseObject { get; set; }

        [DataMember]
        public string User { get; set; }

        //public DateTime DateTimeStamp { get; set; }

        //public string UserAgent { get; set; }
        //public string OS { get; set; }
        //public string Device { get; set; }
        //public string OSVersion { get; set; }
        //public string BrowserVersion { get; set; }

        #endregion

    }

}
