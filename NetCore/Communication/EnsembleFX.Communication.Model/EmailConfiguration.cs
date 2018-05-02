

namespace EnsembleFX.Communication.Model
{
    using System.Runtime.Serialization;

    [DataContract]
    public class EmailConfiguration
    {
        #region Constructor
        public EmailConfiguration()
        {
            EmailType = "text/html";
        }
        #endregion

        #region Public Properties

        [DataMember]
        public string FromAddress { get; set; }

        [DataMember]
        public string EmailType { get; set; }

        [DataMember]
        public string Subject { get; set; }

        [DataMember]
        public bool IsEnabled { get; set; }

        [DataMember]
        public string Blacklist { get; set; }

        [DataMember]
        public string WhiteList { get; set; }

        [DataMember]
        public string To { get; set; }

        [DataMember]
        public string CC { get; set; }

        [DataMember]
        public string BCC { get; set; }

        #endregion
    }
}
