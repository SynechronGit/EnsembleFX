using System;
using System.Linq;
using System.Configuration;

namespace EnsembleFX.Logging.Configuration
{
    public class LoggersConfigurationSection : ConfigurationSection
    {
        internal const string LoggersSectionName = "ensemble.Loggers";

        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public LoggerSettingCollection LoggerConfigurationSettings
        {
            get { return (LoggerSettingCollection)this[""]; }
            set { this[""] = value; }
        }

        public static LoggersConfigurationSection GetCurrent()
        {
            return (LoggersConfigurationSection)ConfigurationManager.GetSection(@"ensemble/" + LoggersSectionName);
        }
    }
}
