using System;
using System.Linq;
using System.Configuration;

namespace EnsembleFX.Logging.Configuration
{
    [ConfigurationCollection(typeof(LoggerConfigurationSetting), AddItemName = "logger")]
    public class LoggerSettingCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LoggerConfigurationSetting();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LoggerConfigurationSetting)element).Name;
        }
    }
}
