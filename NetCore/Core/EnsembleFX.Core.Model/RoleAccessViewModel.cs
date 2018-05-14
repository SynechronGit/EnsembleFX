using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Shared
{
  
    [Serializable]
    public class RoleAccessViewModel
    {
        public Guid _id { get; set; }
        public int RoleId { get; set; }
        //public string RoleId { get; set; }
        public bool CanWrite { get; set; }
        public string PermissionName { get; set; }
        public bool CanRead { get; set; }
    }
}
