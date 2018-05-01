
namespace EnsembleFX.Configuration.Model
{
    using System;
    using System.Configuration;
    public class BlackListElement : ConfigurationElement
    {
        [ConfigurationProperty("ids", DefaultValue = "", IsRequired = false)]
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
    }
}
