using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace EnsembleFX.Core.Security
{

   
    public class EnsembleContext
    {
        private IHttpContextAccessor _contextAccessor;

        public EnsembleContext(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        private HttpContext Context { get { return _contextAccessor.HttpContext; } }
        public string Email
        {
            get
            {
                return ((ClaimsIdentity)Context.User.Identity).FindFirst(ClaimTypes.Email).Value; 
            }
        }
        public string Name
        {
            get
            {
               
                var claim = ((ClaimsIdentity)Context.User.Identity).FindFirst(ClaimTypes.Name); 

                if (null != claim)
                {
                    return claim.Value;
                }
                else
                {
                    return string.Empty;
                }

            }
        }
        public string UserId
        {
            get
            {
                return ((ClaimsIdentity)Context.User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value; 
               
            }
        }
        public string ApplicationIdentifier
        {
            get
            {
                return ((ClaimsIdentity)Context.User.Identity).FindFirst("ApplicationIdentifier").Value;
               
            }
        }
        public string EnvironmentIdentifier
        {
            get
            {
                return ((ClaimsIdentity)Context.User.Identity).FindFirst("EnvironmentIdentifier").Value;
              
            }
        }

        public string ClientId
        {
            get
            {

                var clientID = ((ClaimsIdentity)Context.User.Identity).FindFirst("ClientId"); 

                if (clientID != null)
                    return clientID.Value;

                return "";
            }
        }
    }
}