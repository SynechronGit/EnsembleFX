using EnsembleFX.StorageAdapter.Abstractions;
using EnsembleFX.StorageAdapter.Audit.Abstractions;
using EnsembleFX.StorageAdapter.Model.Audit.Abstractions;
using EnsembleFX.StorageAdapter.Model.Audit.TableStore;
using System.Threading.Tasks;

namespace EnsembleFX.StorageAdapter.Audit.TableStore
{
    /// <summary>
    /// Class AzureTableAuditStorageTrailProvider IAuditTrailProvider interface to save audit to the database and query audit from the database.
    /// </summary>
    public class AzureTableAuditStorageTrailProvider : IAuditTrailProvider
	{
		#region Constructors
		/// <summary>
		/// TableStorageAdapter constructor injection
		/// </summary>
		private IAzureStorageTableAdapter<AzureTableAuditLog> _azureStorageTableAdapter;
		public AzureTableAuditStorageTrailProvider(IAzureStorageTableAdapter<AzureTableAuditLog> azureStorageTableAdapter)
		{
			this._azureStorageTableAdapter = azureStorageTableAdapter;

		}
		#endregion

		#region IAuditTrailProvider Implementation
		/// <summary>
		/// Inserts AuditLog into the database
		/// </summary>
		/// <param name="auditTrailLog"></param>
		/// <returns> <c>True</c> if log is saved succesfully into the database; otherwise, <c>false</c></returns>
		public async Task<bool> AddLogAsync(IAuditTrailLog auditTrailLog)
		{
			if (auditTrailLog != null)
			{
				await _azureStorageTableAdapter.InsertAsync((AzureTableAuditLog)auditTrailLog);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Queries AuditLog in the database
		/// </summary>
		/// <param name="auditTrailPaging"></param>
		/// <returns>Query result defined by AuditPaging range</returns>
		public async Task<IAuditTrailLog> QueryAuditLogsAsync(IAuditTrailPaging auditTrailPaging)
		{
			if (auditTrailPaging != null)
			{				
				var result = await _azureStorageTableAdapter.GetAllAsync();
				/*
				return await _dsStorageAdapter.QueryAudit(auditTrailPaging.Start, auditTrailPaging.End);
				*/
			}
			return null;
		}
		#endregion
	}
}

