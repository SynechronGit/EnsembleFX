using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.BuildingBlocks
{
	/// <summary>
	/// Represents Audit Trail properties for an entity type
	/// </summary>
	public interface IAuditTrail
	{
		/// <summary>
		/// Gets or Sets the CreatedBy
		/// </summary>
		string CreatedBy { get; set; }
		/// <summary>
		/// Gets or Sets the CreatedOn
		/// </summary>
		DateTime CreatedOn { get; set; }
		/// <summary>
		/// Gets or Sets the ModifiedBy
		/// </summary>
		string ModifiedBy { get; set; }
		/// <summary>
		/// Gets or Sets the ModifiedOn
		/// </summary>
		DateTime ModifiedOn { get; set; }
		/// <summary>
		/// Gets or Sets the DeletedBy 
		/// </summary>
		string DeletedBy { get; set; }
		/// <summary>
		/// Gets or Sets the DeletedOn
		/// </summary>
		DateTime? DeletedOn { get; set; }
	}
}
