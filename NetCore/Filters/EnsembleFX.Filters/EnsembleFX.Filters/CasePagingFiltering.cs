using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Filters
{
    public class CasePagingFiltering : PagingFiltering
    {
        public int DocumentTypeId { get; set; }
        public string environment { get; set; }
        public string roles { get; set; }
    }
}
