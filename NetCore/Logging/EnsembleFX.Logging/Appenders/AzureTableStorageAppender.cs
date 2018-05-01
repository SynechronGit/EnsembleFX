using EnsembleFX.Logging.Entities;
using EnsembleFX.StorageAdapter;
using log4net.Appender;
using log4net.Core;
using Microsoft.WindowsAzure.Storage.Table;
using  System.Threading.Tasks;
namespace EnsembleFX.Logging.Appenders
{
    public class AzureTableStorageAppender : AppenderSkeleton
    {
        #region Private Variables
        private AzureStorageTableAdapter<ApplicationLogs> azureStorageTableAdapter;
        #endregion

        public AzureTableStorageAppender()
        {
            azureStorageTableAdapter = new AzureStorageTableAdapter<ApplicationLogs>();
            //LogEntityType = typeof(ApplicationLogs);
        }

        protected async override void Append(LoggingEvent loggingEvent)
        {
            ApplicationLogs appLogs = new ApplicationLogs();
            appLogs.Message = loggingEvent.RenderedMessage;
            appLogs.Timestamp = loggingEvent.TimeStamp;
            appLogs.Thread = loggingEvent.ThreadName;
            appLogs.LogLevelType = loggingEvent.Level.Name;
            appLogs.LoggerName = loggingEvent.LoggerName;
            appLogs.Message = loggingEvent.RenderedMessage;
            appLogs.Exception = loggingEvent.ExceptionObject;
            appLogs.Environment = (log4net.LogicalThreadContext.Properties["Environment"] != null) ? log4net.LogicalThreadContext.Properties["Environment"].ToString() : string.Empty;
            appLogs.User = (log4net.LogicalThreadContext.Properties["User"] != null) ? log4net.LogicalThreadContext.Properties["User"].ToString() : string.Empty;
            appLogs.UrlReferrer = (log4net.LogicalThreadContext.Properties["UrlReferrer"] != null) ? log4net.LogicalThreadContext.Properties["UrlReferrer"].ToString() : string.Empty;
            appLogs.ClientBrowser = (log4net.LogicalThreadContext.Properties["ClientBrowser"] != null) ? log4net.LogicalThreadContext.Properties["ClientBrowser"].ToString() : string.Empty;
            appLogs.ClientIP = (log4net.LogicalThreadContext.Properties["ClientIP"] != null) ? log4net.LogicalThreadContext.Properties["ClientIP"].ToString() : string.Empty;
            appLogs.URL = (log4net.LogicalThreadContext.Properties["URL"] != null) ? log4net.LogicalThreadContext.Properties["URL"].ToString() : string.Empty;
            appLogs.ApplicationIdentifier = (log4net.LogicalThreadContext.Properties["ApplicationIdentifier"] != null) ? log4net.LogicalThreadContext.Properties["ApplicationIdentifier"].ToString() : string.Empty;
            appLogs.Source = (log4net.LogicalThreadContext.Properties["Source"] != null) ? log4net.LogicalThreadContext.Properties["Source"].ToString() : string.Empty;
            appLogs.RequestObject = (log4net.LogicalThreadContext.Properties["RequestObject"] != null) ? log4net.LogicalThreadContext.Properties["RequestObject"].ToString() : string.Empty;
            appLogs.EventName = (log4net.LogicalThreadContext.Properties["EventName"] != null) ? log4net.LogicalThreadContext.Properties["EventName"].ToString() : string.Empty;
            appLogs.StackTrace = (log4net.LogicalThreadContext.Properties["StackTrace"] != null) ? log4net.LogicalThreadContext.Properties["StackTrace"].ToString() : string.Empty;
            appLogs.ResponseObject = (log4net.LogicalThreadContext.Properties["ResponseObject"] != null) ? log4net.LogicalThreadContext.Properties["ResponseObject"].ToString() : string.Empty;
            //appLogs.UserAgent = (log4net.LogicalThreadContext.Properties["UserAgent"] != null) ? log4net.LogicalThreadContext.Properties["UserAgent"].ToString() : string.Empty;
            //appLogs.OS = (log4net.LogicalThreadContext.Properties["OS"] != null) ? log4net.LogicalThreadContext.Properties["OS"].ToString() : string.Empty;
            //appLogs.Device = (log4net.LogicalThreadContext.Properties["Device"] != null) ? log4net.LogicalThreadContext.Properties["Device"].ToString() : string.Empty;
            //appLogs.OSVersion = (log4net.LogicalThreadContext.Properties["OSVersion"] != null) ? log4net.LogicalThreadContext.Properties["OSVersion"].ToString() : string.Empty;
            //appLogs.BrowserVersion = (log4net.LogicalThreadContext.Properties["BrowserVersion"] != null) ? log4net.LogicalThreadContext.Properties["BrowserVersion"].ToString() : string.Empty;
            //azureStorageTableAdapter.InsertTable<ApplicationLogs>("ApplicationLogs", appLogs);
            await azureStorageTableAdapter.InsertAsync(appLogs);
        }
    }
}
