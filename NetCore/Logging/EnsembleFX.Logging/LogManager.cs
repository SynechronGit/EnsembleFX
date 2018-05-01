using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnsembleFX.Core.Dependency;
using EnsembleFX.Logging.Entities;
using EnsembleFX.Logging.Enums;

namespace EnsembleFX.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class LogManager : ILogManager
    {
        #region Private Members
        /// <summary>
        /// The controller
        /// </summary>
        private static ILogController _sController;

        /// <summary>
        /// The log manager instance
        /// </summary>
        private static LogManager _logManagerInstance;

        /// <summary>
        /// Gets the log manager.
        /// </summary>
        /// <returns></returns>
        #endregion

        #region Public Methods
        public LogManager GetLogManager()
        {
                  return this;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="LogManager"/> class from being created.
        /// </summary>
        public LogManager(ILogController logController)
        {
            if (_sController == null)
            {
                _sController = logController;//new DependencyManager().Resolve<ILogController>();
            }
        }

        /// <summary>
        /// Logs the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="logLevel">The log level.</param>
        public void LogMessage(string message, string userName, string environment, LogLevel logLevel = LogLevel.Info)
        {
            LogMessage(message, userName, environment, null, logLevel);
        }

        /// <summary>
        /// Logs the message long with an exception object.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="logLevel">The log level.</param>
        public void LogMessage(string message, string userName, string environment, Exception ex, LogLevel logLevel = LogLevel.Error)
        {
            try
            {
                log4net.LogicalThreadContext.Properties["Environment"] = environment == null ? "NA" : environment;
                log4net.LogicalThreadContext.Properties["User"] = userName;
                log4net.LogicalThreadContext.Properties["UrlReferrer"] = string.Empty;
                log4net.LogicalThreadContext.Properties["ClientBrowser"] = string.Empty;
                log4net.LogicalThreadContext.Properties["ClientIP"] = string.Empty;
                log4net.LogicalThreadContext.Properties["URL"] = string.Empty;
                log4net.LogicalThreadContext.Properties["ApplicationIdentifier"] = string.Empty;
                log4net.LogicalThreadContext.Properties["Source"] = string.Empty;
                log4net.LogicalThreadContext.Properties["RequestObject"] = string.Empty;
                log4net.LogicalThreadContext.Properties["EventName"] = string.Empty;
                string exceptionMessage = ex != null ? string.Format("ExceptionMessage-{0}", ex.Message) : string.Empty;

                _sController.Log(new ApplicationLogs()
                {
                    LogLevel = logLevel,
                    Exception = ex,
                    Message = String.Format(CultureInfo.CurrentCulture, "MESSAGE - {0} {1}",
                                            message, exceptionMessage),
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception)
            { }
        }

        public void LogMessage(LogModel log, LogLevel logLevel = LogLevel.Info, string requestObject = "", string eventName = "")
        {
            LogMessage(log, null, logLevel, requestObject, eventName);
        }

        public void LogMessage(LogModel log, Exception ex, LogLevel logLevel = LogLevel.Error, string requestObject = "", string eventName = "")
        {
            try
            {
                log4net.LogicalThreadContext.Properties["Environment"] = log.Environment == null ? "NA" : log.Environment;
                log4net.LogicalThreadContext.Properties["User"] = log.UserName;
                log4net.LogicalThreadContext.Properties["UrlReferrer"] = log.UrlReferrer == null ? "" : log.UrlReferrer;
                log4net.LogicalThreadContext.Properties["ClientBrowser"] = log.ClientBrowser == null ? "" : log.ClientBrowser;
                log4net.LogicalThreadContext.Properties["ClientIP"] = log.ClientIP == null ? "" : log.ClientIP;
                log4net.LogicalThreadContext.Properties["URL"] = log.Url == null ? "" : log.Url;
                log4net.LogicalThreadContext.Properties["ApplicationIdentifier"] = log.ApplicationIdentifier;
                log4net.LogicalThreadContext.Properties["Source"] = log.Source == null ? "" : log.Source;
                log4net.LogicalThreadContext.Properties["RequestObject"] = log.RequestObject == null ? "" : log.RequestObject;
                log4net.LogicalThreadContext.Properties["EventName"] = log.EventName == null ? "" : log.EventName;
                log4net.LogicalThreadContext.Properties["StackTrace"] = log.StackTrace == null ? "" : log.StackTrace;
                log4net.LogicalThreadContext.Properties["ResponseObject"] = log.ResponseObject == null ? "" : log.ResponseObject;
                string exceptionMessage = string.Empty;
                if (ex != null)
                {
                    switch (ex.GetType().FullName)
                    {
                        case "System.Data.Entity.Validation.DbEntityValidationException":
                            System.Data.Entity.Validation.DbEntityValidationException validation = ex as System.Data.Entity.Validation.DbEntityValidationException;
                            exceptionMessage = "ExceptionMessage-";
                            if (validation != null && validation.EntityValidationErrors != null)
                            {
                                foreach (var validationErrors in validation.EntityValidationErrors)
                                {
                                    foreach (var validationError in validationErrors.ValidationErrors)
                                    {
                                        exceptionMessage += string.Format(" Property: {0} Error: {1} ", validationError.PropertyName, validationError.ErrorMessage);
                                    }
                                }

                            }


                            break;
                    }
                }


                _sController.Log(new ApplicationLogs()
                {
                    LogLevel = logLevel,
                    Exception = ex,
                    Message = String.Format(CultureInfo.CurrentCulture, "{0} {1}",
                                            log.Message, exceptionMessage),
                    Timestamp = DateTime.UtcNow,
                    StackTrace = log4net.LogicalThreadContext.Properties["StackTrace"].ToString()
                });
            }
            catch (Exception)
            { }
        }
        #endregion
    }
}
