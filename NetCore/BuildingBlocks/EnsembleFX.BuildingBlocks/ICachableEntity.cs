using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.BuildingBlocks
{
	/// <summary>
	/// Denotes entity as cachable if implemented
	/// </summary>
	public interface ICachableEntity
	{
		/// <summary>
		/// Hash of entity instance
		/// </summary>
		string ETag { get; set; }
	}
}
