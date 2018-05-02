
namespace EnsembleFX.Communication.Model
{
    using System.Configuration;
    public class MessageElement : ConfigurationElement
    {
        [ConfigurationProperty("to")]
        public MessageTo To
        {
            get
            {
                return (MessageTo)this["to"];
            }
            set
            {
                this["to"] = value;
            }
        }

        [ConfigurationProperty("cc")]
        public MessageCC CC
        {
            get
            {
                return (MessageCC)this["cc"];
            }
            set
            {
                this["cc"] = value;
            }
        }

        [ConfigurationProperty("bcc")]
        public MessageBCC BCC
        {
            get
            {
                return (MessageBCC)this["bcc"];
            }
            set
            {
                this["bcc"] = value;
            }
        }

        [ConfigurationProperty("subject")]
        public MessageSubject Subject
        {
            get
            {
                return (MessageSubject)this["subject"];
            }
            set
            {
                this["subject"] = value;
            }
        }
    }
}
