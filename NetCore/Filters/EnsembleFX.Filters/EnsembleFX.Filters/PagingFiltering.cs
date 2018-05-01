using System.Collections.Generic;

namespace EnsembleFX.Filters
{
    public class PagingFiltering
    {
        public int skip { get; set; }
        public int take { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
        public GridFilters filter { get; set; }
        public List<GridSort> Sort { get; set; }
        public string environment { get; set; }
        public string Name { get; set; }
        public List<GridColumn> columns{ get; set; }
    }

    public class GridColumn
    {
        public string column { get; set; }
        public string title { get; set; }
    }
}
