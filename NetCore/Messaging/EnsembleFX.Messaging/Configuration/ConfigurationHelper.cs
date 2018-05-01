using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Configuration
{
    public static class ConfigurationHelper
    {
        #region Private Properties
        static string[] _allowed = { };
        #endregion

        #region Constructor
        static ConfigurationHelper()
        {
        }
        #endregion

        #region Public Methods

        public static bool IsEnvironmentInAllowedList(IMessageEnvelope messageEnvelope)
        {
            bool result = false;
            if (string.IsNullOrEmpty(messageEnvelope.AllowedEnvironments))
            {
                result = true;
            }
            else
            {
                _allowed = messageEnvelope.AllowedEnvironments.Split(',');
                if (_allowed.Any(env => env.Trim().Equals(messageEnvelope.Environment, StringComparison.InvariantCultureIgnoreCase)))
                    result = true;
            }

            return result;
        }

        public static string GetEnvironment(IMessageEnvelope envelope)
        {
            string environment = string.Empty;
            if (envelope != null)
            {
                if (!string.IsNullOrEmpty(envelope.Environment))
                    environment = envelope.Environment;
            }

            return environment;
        }
        #endregion
    }
}
