using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Shared
{
    public class EnvironmentViewModel
    {
        public Guid _id { get; set; }
        public int EnvironmentID { get; set; }
        public string EnvironmentName { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public string RequestURL { get; set; }
        public int ApplicationID { get; set; }
        public string MicroSiteURL { get; set; }
        public string Application { get; set; }
        public ApplicationViewModel ApplicationViewModel { get; set; }
    }
}
