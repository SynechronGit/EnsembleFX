using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Filters
{
    public class DashboardSettingPagingFiltering : PagingFiltering
    {
        public string DashboardName { get; set; }
        public int UserId { get; set; }
    }
}
