
namespace EnsembleFX.Communication.Model
{
    using System;
    using System.Configuration;
    public class MessageSubject : ConfigurationElement
    {
        [ConfigurationProperty("text")]
        public String Text
        {
            get
            {
                return (String)this["text"];
            }
            set
            {
                this["text"] = value;
            }
        }

        [ConfigurationProperty("replaceOrAppend")]
        public String ReplaceOrAppend
        {
            get
            {
                return (String)this["replaceOrAppend"];
            }
            set
            {
                this["replaceOrAppend"] = value;
            }
        }
    }
}
