
using System.Collections.Generic;
namespace EnsembleFX.Core.Filters
{
    public class TokenPagingFiltering : PagingFiltering
    {
        public string UserId { get; set; }
        public int skip { get; set; }
        public int take { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public GridFilters filter { get; set; }
        public List<GridSort> Sort { get; set; }
    }
}
