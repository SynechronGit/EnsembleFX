using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace EnsembleFX.Logging.Configuration
{
    public sealed class LoggingConfigurationSetting : ConfigurationElement
    {
        [ConfigurationProperty("mode", DefaultValue = "Synchronous", IsRequired = true)]
        public string Mode
        {
            get
            {
                return (string)base["mode"];
            }
        }

        [ConfigurationProperty("sleepInterval", DefaultValue = 1000, IsRequired = true)]
        public int SleepInterval
        {
            get
            {
                return (int)base["sleepInterval"];
            }
        }

        [ConfigurationProperty("maxQueueSize", DefaultValue = 100, IsRequired = true)]
        public int MaxQueueSize
        {
            get
            {
                return (int)base["maxQueueSize"];
            }
        }

    }
}
