using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using EnsembleFX.Logging.Entities;
// TODO : remove circular dependency between repository and logging
//using EnsembleFX.Repository; 

namespace EnsembleFX.Logging.Appenders
{
    public class MongoAppender : AppenderSkeleton
    {
        #region Private Variables
        // TODO : remove circular dependency between repository and logging
        //private  IDBRepository<ApplicationLogs> _dbRepository;
        #endregion

        public MongoAppender()
        {
            
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            ApplicationLogs appLogs = new ApplicationLogs();
            appLogs.Message = loggingEvent.RenderedMessage;          
            //appLogs.DateTimeStamp = DateTimeOffset.Now.UtcDateTime;
            //appLogs.UserAgent = (log4net.LogicalThreadContext.Properties["UserAgent"] != null) ? log4net.LogicalThreadContext.Properties["UserAgent"].ToString() : string.Empty;
            //appLogs.OS = (log4net.LogicalThreadContext.Properties["OS"] != null) ? log4net.LogicalThreadContext.Properties["OS"].ToString() : string.Empty;
            //appLogs.Device = (log4net.LogicalThreadContext.Properties["Device"] != null) ? log4net.LogicalThreadContext.Properties["Device"].ToString() : string.Empty;
            //appLogs.OSVersion = (log4net.LogicalThreadContext.Properties["OSVersion"] != null) ? log4net.LogicalThreadContext.Properties["OSVersion"].ToString() : string.Empty;
            //appLogs.BrowserVersion = (log4net.LogicalThreadContext.Properties["BrowserVersion"] != null) ? log4net.LogicalThreadContext.Properties["BrowserVersion"].ToString() : string.Empty;

            appLogs.Thread = loggingEvent.ThreadName;
            appLogs.LogLevelType = loggingEvent.Level.Name;
            appLogs.LoggerName = loggingEvent.LoggerName;         
            appLogs.Exception = loggingEvent.ExceptionObject;
            //appLogs.Environment = (log4net.LogicalThreadContext.Properties["Environment"] != null) ? log4net.LogicalThreadContext.Properties["Environment"].ToString() : string.Empty;
            //appLogs.User = (log4net.LogicalThreadContext.Properties["User"] != null) ? log4net.LogicalThreadContext.Properties["User"].ToString() : string.Empty;
            //appLogs.UrlReferrer = (log4net.LogicalThreadContext.Properties["UrlReferrer"] != null) ? log4net.LogicalThreadContext.Properties["UrlReferrer"].ToString() : string.Empty;
            //appLogs.ClientBrowser = (log4net.LogicalThreadContext.Properties["ClientBrowser"] != null) ? log4net.LogicalThreadContext.Properties["ClientBrowser"].ToString() : string.Empty;
            //appLogs.ClientIP = (log4net.LogicalThreadContext.Properties["ClientIP"] != null) ? log4net.LogicalThreadContext.Properties["ClientIP"].ToString() : string.Empty;
            //appLogs.URL = log4net.LogicalThreadContext.Properties["URL"].ToString(); //(log4net.LogicalThreadContext.Properties["URL"] != null || log4net.LogicalThreadContext.Properties["URL"].ToString() == string.Empty) ? log4net.LogicalThreadContext.Properties["URL"].ToString() : string.Empty;
            //appLogs.ApplicationIdentifier = (log4net.LogicalThreadContext.Properties["ApplicationIdentifier"] != null) ? log4net.LogicalThreadContext.Properties["ApplicationIdentifier"].ToString() : string.Empty;
            //appLogs.Source = (log4net.LogicalThreadContext.Properties["Source"] != null) ? log4net.LogicalThreadContext.Properties["Source"].ToString() : string.Empty;
            //appLogs.RequestObject = (log4net.LogicalThreadContext.Properties["RequestObject"] != null) ? log4net.LogicalThreadContext.Properties["RequestObject"].ToString() : string.Empty;
            //appLogs.EventName = (log4net.LogicalThreadContext.Properties["EventName"] != null) ? log4net.LogicalThreadContext.Properties["EventName"].ToString() : string.Empty;
            appLogs.StackTrace = (log4net.LogicalThreadContext.Properties["StackTrace"] != null) ? log4net.LogicalThreadContext.Properties["StackTrace"].ToString() : string.Empty;
            // TODO : remove circular dependency between repository and logging
            /*
            _dbRepository = new MongoDbRepository<ApplicationLogs>("applogs");
            _dbRepository.Insert(appLogs);
            */
        }
    }
}
