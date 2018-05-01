using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnsembleFX.Logging.Enums;

namespace EnsembleFX.Logging
{
    public interface ILogManager
    {
        LogManager GetLogManager();
        void LogMessage(LogModel log, LogLevel logLevel = LogLevel.Info, string requestObject = "", string eventName = "");
        void LogMessage(LogModel log, Exception ex, LogLevel logLevel = LogLevel.Error, string requestObject = "", string eventName = "");
        void LogMessage(string message, string userName, string environment, LogLevel logLevel = LogLevel.Info);
        void LogMessage(string message, string userName, string environment, Exception ex, LogLevel logLevel = LogLevel.Error);
    }
}
