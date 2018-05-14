using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EnsembleFX.Shared
{

    public class UserProfile
    {
        #region Public Properties
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string UserDomain { get; set; }
        public string RoleType { get; set; }
        public string AppKey { get; set; }
        public List<int> Roles { get; set; }
        public List<string> RoleNames { get; set; }
        public List<RoleAccessViewModel> RoleAccessInfo { get; set; }
        public bool HasRoleAccess { get; set; }
        public string CurrentEnvironment { get; set; }
        public List<EnvironmentViewModel> Environments { get; set; }
        public DateTime LoggedInOn { get; set; }
        #endregion

        #region Public Methods
        public string CurrentApplication { get; set; }
        public bool IsUserInAdminRole()
        {
            if (Roles != null && Roles.Count > 0)
            {
                return Roles.Exists(r => r == (int)EnsembleFX.ApiProxy.Models.RoleType.Admin);
            }
            return false;
        }
        public bool HasAccess(string controllerName, string actionName)
        {
            bool result = false;
            var moduleName = controllerName + "-" + actionName;
            if (RoleAccessInfo != null)
            {
                var checkControllerActionRole = RoleAccessInfo.Find(ra => ra.PermissionName.Equals(moduleName, StringComparison.InvariantCultureIgnoreCase) && (ra.CanWrite == true || ra.CanRead == true));
                if (checkControllerActionRole != null)
                    result = true;
                else
                {
                    var checkControllerRole = RoleAccessInfo.Find(ra => ra.PermissionName.Equals(controllerName, StringComparison.InvariantCultureIgnoreCase) && (ra.CanWrite == true || ra.CanRead == true));
                    if (checkControllerRole != null)
                        result = true;
                }
            }
            return result;
        }

        public bool HasWriteAccess(string moduleName)
        {
            bool hasWriteAccess = false;
            List<RoleAccessViewModel> roleAccess = null;

            if (RoleAccessInfo == null && RoleAccessInfo.Count <= 0)
                return hasWriteAccess;

            roleAccess = RoleAccessInfo.FindAll(ra => ra.PermissionName.Equals(moduleName, StringComparison.InvariantCultureIgnoreCase)
                && ra.CanWrite == true);

            if (roleAccess != null && roleAccess.Count > 0)
            {
                hasWriteAccess = true;
            }

            return hasWriteAccess;
        }
        #endregion
    }
}
