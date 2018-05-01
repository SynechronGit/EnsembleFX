using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public class LogModel
    {
        #region Public Properties
        public string ApplicationIdentifier { get; set; }
        public string Message { get; set; }
        public string Environment { get; set; }
        public string UserName { get; set; }
        public string WebServer { get; set; }
        public string ClientBrowser { get; set; }
        public string ClientIP { get; set; }
        public string Url { get; set; }
        public string UrlReferrer { get; set; }
        public string Source { get; set; }
        public string EventName { get; set; }
        public string RequestObject { get; set; }
        public string ResponseObject { get; set; }
        public string StackTrace { get; set; }
        #endregion
    }
}
