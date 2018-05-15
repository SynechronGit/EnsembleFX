using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.BuildingBlocks
{
	/// <summary>
	/// Data Query model
	/// </summary>
	public interface IQueryModel
	{
		/// <summary>
		/// Filter from $filter
		/// </summary>
		string Filter { get; set; }
		/// <summary>
		/// Order by clause given as $orderby
		/// </summary>
		string OrderBy { get; set; }
		/// <summary>
		/// Shows Top number of record as $top
		/// </summary>
		int Top { get; set; }

		/// <summary>
		/// Skips initial number of records as $skip
		/// </summary>
		int Skip { get; set; }
		/// <summary>
		/// Convert Query to string
		/// </summary>
		/// <returns>String representation of Query</returns>
		string ToString();
	}
}
