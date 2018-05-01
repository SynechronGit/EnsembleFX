using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnsembleFX.Logging.Enums
{
    /// <summary>
    /// Enum to specify logging mode
    /// </summary>
    public enum LogMode
    {
        /// <summary>
        /// Logs Synchronously on same thread
        /// </summary>
        Synchronous = 0,
        /// <summary>
        /// Logs asynchronously on different thread
        /// </summary>
        Asynchronous = 1
    }
}
