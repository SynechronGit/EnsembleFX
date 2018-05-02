using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Microsoft.AspNetCore.Http;

namespace EnsembleFX.Helper
{
    public class OperationHelper : IOperationHelper
    {
        private IHttpContextAccessor httpContextAccessor;
        public OperationHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        
        /// <summary>
        /// Get logged in Username
        /// </summary>
        /// <returns>logged in Username</returns>
        public string GetUserName()
        {
            var HttpContext = this.httpContextAccessor.HttpContext;

            return (HttpContext.User != null &&
                    HttpContext.User.Identity != null &&
                    !string.IsNullOrEmpty(HttpContext.User.Identity.Name)) ?
                        HttpContext.User.Identity.Name : string.Empty;
        }
    }
}