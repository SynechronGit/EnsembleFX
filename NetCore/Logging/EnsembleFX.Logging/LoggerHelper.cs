using System.Net.Http;
using System.Security.Principal;
using  Microsoft.AspNetCore.Http;
namespace EnsembleFX.Logging
{
    public class LoggerHelper
    {
        private IHttpContextAccessor httpContextAccessor;
        public LoggerHelper(IHttpContextAccessor httpContextAccessor){
            this.httpContextAccessor = httpContextAccessor;
        }
        #region Public Methods

        public static LogModel GetLogModel(HttpRequestMessage request, string environment, string userName, string message, string applicationIdentifier = "")
        {
            return new LogModel
            {
                ClientBrowser = GetBrowserInfo(request),
                ClientIP = GetClientIP(request),
                WebServer = GetHostInfo(request),
                Url = request != null && request.RequestUri != null ? request.RequestUri.AbsoluteUri : string.Empty,
                UrlReferrer = GetReferer(request),
                Environment = environment,
                UserName = userName,
                Message = message,
                ApplicationIdentifier = !string.IsNullOrEmpty(applicationIdentifier) ? applicationIdentifier : string.Empty
            };
        }


        public static LogModel GetLogModel(string environment, string userName, string message, string source)
        {
            return new LogModel
            {
                Environment = environment,
                UserName = userName,
                Message = message,
                Source = source
            };
        }


        public static string GetBrowserInfo(HttpRequestMessage request)
        {
            string browser = string.Empty;
            // TODO Need to see how to get browser info in .net core
            /*
            if (request != null && request.Properties.ContainsKey("MS_HttpContext"))
            {
                var context = request.Properties["MS_HttpContext"] as HttpContextWrapper;
                browser = context != null && context.Request != null && context.Request.Browser != null ?
                    string.Format("{0}-{1}", context.Request.Browser.Browser, context.Request.Browser.Version) :
                    string.Empty;

            }
             */
            return browser;
        }

        public static string GetClientIP(HttpRequestMessage request)
        {

            string clientIp = string.Empty;
            // TODO Need to see how to get user host address in .net core
            /*
            if (request != null && request.Properties.ContainsKey("MS_HttpContext"))
            {
                var context = request.Properties["MS_HttpContext"] as HttpContextWrapper;
                clientIp = context != null && context.Request != null ? context.Request.UserHostAddress : string.Empty;
            }
            */
            return clientIp;
        }

        public static string GetHostInfo(HttpRequestMessage request)
        {
            string host = string.Empty;
              // TODO Need to see how to get user host info in .net core
            /*
            if (request != null && request.Properties.ContainsKey("MS_HttpContext"))
            {
                var context = request.Properties["MS_HttpContext"] as HttpContextWrapper;
                host = context != null && context.Request != null ? context.Request.UserHostName : string.Empty;
            }
            */
            return host;
        }

        public static string GetReferer(HttpRequestMessage request)
        {
            string referrer = string.Empty;
              // TODO Need to see how to get user referer in .net core
            /*
            if (request != null && request.Properties.ContainsKey("MS_HttpContext"))
            {
                var context = request.Properties["MS_HttpContext"] as HttpContextWrapper;
                referrer = context != null && context.Request != null && context.Request.UrlReferrer != null ? context.Request.UrlReferrer.AbsoluteUri : string.Empty;
            }
            */
            return referrer;
        }

        public static string GetUser(IPrincipal user)
        {
            return user != null && user.Identity != null ? user.Identity.Name : string.Empty;
        }

        #endregion
    }
}
