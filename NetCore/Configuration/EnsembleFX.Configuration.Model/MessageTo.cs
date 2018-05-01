
namespace EnsembleFX.Configuration.Model
{
    using System;
    using System.Configuration;
    public class MessageTo : ConfigurationElement
    {
        [ConfigurationProperty("ids")]
        public String Ids
        {
            get
            {
                return (String)this["ids"];
            }
            set
            {
                this["ids"] = value;
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
