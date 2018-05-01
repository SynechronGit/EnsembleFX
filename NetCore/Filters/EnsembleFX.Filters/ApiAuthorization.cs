//TODO:: Need to solved ApiAuthorization errors

//using EnsembleFX.Core.Security;
//using EnsembleFX.Shared;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Web;
//using System.Web.Http;
//using EnsembleFX.ApiProxy;

//namespace EnsembleFX.Filters
//{
//    public class ApiAuthorization : AuthorizeAttribute
//    {
//        public string PermissionName { get; set; }
//        public RuntimeApiProxy _runtimeApiProxy { get; set; }

//        /// <summary>
//        /// Checks whether the user is authorized to do the requested operation
//        /// </summary>
//        /// <param name="actionContext"> the current Http Request Context</param>
//        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
//        {
//            _runtimeApiProxy = new RuntimeApiProxy();
//            HttpMethod requestType = null;

//            if (string.IsNullOrEmpty(this.PermissionName))
//            {
//                base.HandleUnauthorizedRequest(actionContext);
//            }
//            else
//            {
//                if (actionContext.Request != null)
//                    requestType = actionContext.Request.Method;

//                if (_runtimeApiProxy.HasRoleAccessCheck(EnsembleContext.Name, this.PermissionName, requestType))
//                    base.OnAuthorization(actionContext);
//                else
//                    base.HandleUnauthorizedRequest(actionContext);
//            }
//        }

//    }
//}