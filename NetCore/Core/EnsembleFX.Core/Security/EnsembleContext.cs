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
                //return ((ClaimsIdentity)HttpContext.Current.GetOwinContext().Authentication.User.Identity).FindFirst(ClaimTypes.Email).Value; //To Do: .NET Core compatible replacemnt.
                return null;
            }
        }
        public static string Name
        {
            get
            {
                return null;
                //var claim = ((ClaimsIdentity)HttpContext.Current.GetOwinContext().Authentication.User.Identity).FindFirst(ClaimTypes.Name); //To Do: .NET Core compatible replacemnt.
               
                //if (null != claim)
                //{
                //    return claim.Value;
                //}
                //else
                //{
                //    return string.Empty;
                //}

            }
        }
        public static string UserId
        {
            get
            {
                //return ((ClaimsIdentity)HttpContext.Current.GetOwinContext().Authentication.User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value; //To Do: .NET Core compatible replacemnt.
                return null;
            }
        }
        public static string ApplicationIdentifier
        {
            get
            {
                //return ((ClaimsIdentity)HttpContext.Current.GetOwinContext().Authentication.User.Identity).FindFirst("ApplicationIdentifier").Value;//To Do: .NET Core compatible replacemnt.
                return null;
            }
        }
        public static string EnvironmentIdentifier
        {
            get
            {
                //return ((ClaimsIdentity)HttpContext.Current.GetOwinContext().Authentication.User.Identity).FindFirst("EnvironmentIdentifier").Value;//To Do: .NET Core compatible replacemnt.
                return null;
            }
        }

        public static string ClientId
        {
            get
            {
                return null;
                //var clientID = ((ClaimsIdentity)HttpContext.Current.GetOwinContext().Authentication.User.Identity).FindFirst("ClientId"); //To Do: .NET Core compatible replacemnt.

                //if (clientID != null)
                //    return clientID.Value;

                //return "";
            }
        }
    }
}