using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnsembleFX.Core.Security;
using EnsembleFX.Logging.Enums;

namespace EnsembleFX.Logging
{
    public interface ILogItem
    {
        LogLevel LogLevel { get; set; }
        DateTimeOffset Timestamp { get; set; }
        string Title { get; set; }
        string Message { get; set; }
        string LoggerName { get; set; }
        Exception Exception { get; set; }
        int? EventId { get; set; }
        EnterpriseIdentity EnterpriseIdentity {get; set;}
        Dictionary<string, object> ExtensionElements {get; set;}
        string StackTrace { get; set;}
    }
}
