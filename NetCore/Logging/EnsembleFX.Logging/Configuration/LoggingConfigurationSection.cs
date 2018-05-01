using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace EnsembleFX.Logging.Configuration
{
    public sealed class LoggingConfigurationSection : ConfigurationSection
    {
        internal const string ServiceFactorySectionName = "ensemble.Logging";

        [ConfigurationProperty("loggingSettings", IsRequired = false)]
        public LoggingConfigurationSetting LoggingConfigurationSetting
        {
            get { return base["loggingSettings"] as LoggingConfigurationSetting; }
        }

        public static LoggingConfigurationSection GetCurrent()
        {
            return (LoggingConfigurationSection)ConfigurationManager.GetSection(@"ensemble/" + ServiceFactorySectionName);
        }
    }
}
