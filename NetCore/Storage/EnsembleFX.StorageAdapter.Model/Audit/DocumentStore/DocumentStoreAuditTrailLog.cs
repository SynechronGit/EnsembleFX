using EnsembleFX.StorageAdapter.Model.Audit.Abstractions;
using System;

namespace EnsembleFX.StorageAdapter.Model.DocumentStore
{
    /// <summary>
    /// Class DocumentStoreAuditTrailLog implementing IAuditTrailLog interface
    /// </summary>
    public class DocumentStoreAuditTrailLog  : IAuditTrailLog
	{
		/// <summary>
		/// Browser string for AuditLog
		/// </summary>
		public string Browser { get; set; }

		/// <summary>
		/// MachineIP string for AuditLog
		/// </summary>
		public string MachineIP { get; set; }

		/// <summary>
		/// MachineName string for AuditLog
		/// </summary>
		public string MachineName { get; set; }

		/// <summary>
		/// TimeStamp date & time for AuditLog
		/// </summary>
		public DateTime TimeStamp { get; set; }
	}
	
}
