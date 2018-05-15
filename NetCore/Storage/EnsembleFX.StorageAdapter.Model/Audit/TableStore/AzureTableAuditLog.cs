using EnsembleFX.StorageAdapter.Model.Audit.Abstractions;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace EnsembleFX.StorageAdapter.Model.Audit.TableStore
{
    /// <summary>
    /// Class AzureTableAuditLog implementing IAuditTrailLog interface
    /// </summary>
    public class AzureTableAuditLog :  TableEntity , IAuditTrailLog
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
