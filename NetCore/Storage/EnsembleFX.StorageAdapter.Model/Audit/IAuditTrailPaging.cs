namespace EnsembleFX.StorageAdapter.Model.Audit
{
	/// <summary>
	/// Interface declaring start and end range properties to query auditlog
	/// </summary>
	public interface IAuditTrailPaging
	{
		/// <summary>
		/// Start integer for AuditPaging
		/// </summary>
		int Start { get; set; }

		/// <summary>
		/// End integer for AuditPaging
		/// </summary>
		int End { get; set; }
    }
}
