using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace EnsembleFX.Core.Security
{
    public class EnsembleContext
    {
        public static string Email
        {
            get
            {
                return ((ClaimsIdentity)HttpContext.Current.GetOwinContext().Authentication.User.Identity).FindFirst(ClaimTypes.Email).Value;
            }
        }
        public static string Name
        {
            get
            {
                var claim = ((ClaimsIdentity)HttpContext.Current.GetOwinContext().Authentication.User.Identity).FindFirst(ClaimTypes.Name);

                if (null != claim)
                {
                    return claim.Value;
                }
                else
                {
                    return string.Empty;
                }

            //    return ((ClaimsIdentity)HttpContext.Current.GetOwinContext().Authentication.User.Identity).FindFirst(ClaimTypes.Name).Value;
            }
        }
        public static string UserId
        {
            get
            {
                return ((ClaimsIdentity)HttpContext.Current.GetOwinContext().Authentication.User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
            }
        }
        public static string ApplicationIdentifier
        {
            get
            {
                return ((ClaimsIdentity)HttpContext.Current.GetOwinContext().Authentication.User.Identity).FindFirst("ApplicationIdentifier").Value;
            }
        }
        public static string EnvironmentIdentifier
        {
            get
            {
                return ((ClaimsIdentity)HttpContext.Current.GetOwinContext().Authentication.User.Identity).FindFirst("EnvironmentIdentifier").Value;
            }
        }

        public static string ClientId
        {
            get
            {
                var clientID = ((ClaimsIdentity)HttpContext.Current.GetOwinContext().Authentication.User.Identity).FindFirst("ClientId");

                if (clientID != null)
                    return clientID.Value;

                return "";
            }
        }
    }
}