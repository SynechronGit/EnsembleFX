using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Logging
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class LoggingAttribute : Attribute
    {
        public LoggingAttribute()
        {
            LogType = "Info";
        }

        public LoggingAttribute(string logType)
        {
            LogType = logType;
        }

        public string LogType { get; set; }
    }
}
