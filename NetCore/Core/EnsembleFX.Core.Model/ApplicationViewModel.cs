using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Shared
{
    public class ApplicationViewModel
    {
        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public string Version { get; set; }
        public string InstallationType { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string Theme { get; set; }
        public string Title { get; set; }
        public string AppSetting { get; set; } 
    }

   
}
