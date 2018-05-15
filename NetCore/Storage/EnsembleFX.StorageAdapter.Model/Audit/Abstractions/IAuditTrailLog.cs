using System;

namespace EnsembleFX.StorageAdapter.Model.Audit.Abstractions
{
	/// <summary>
	/// Interface declaring Audit properties
	/// </summary>
	public interface IAuditTrailLog
    {
		/// <summary>
		/// Browser string for AuditLog
		/// </summary>
		string Browser { get; set; }

		/// <summary>
		/// MachineIP string for AuditLog
		/// </summary>
		string MachineIP { get; set; }

		/// <summary>
		/// MachineName string for AuditLog
		/// </summary>
		string MachineName { get; set; }

		/// <summary>
		/// TimeStamp date & time for AuditLog
		/// </summary>
		DateTime TimeStamp { get; set; } 
    }
}
