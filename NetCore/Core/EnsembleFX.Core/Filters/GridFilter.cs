using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Core.Filters
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GridFilter
    {
        #region Public Properties
        public string Operator { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public int intValue { get; set; }
        public string AttributeName { get; set; }
        public string DataType { get; set; }
        #endregion
    }
}
