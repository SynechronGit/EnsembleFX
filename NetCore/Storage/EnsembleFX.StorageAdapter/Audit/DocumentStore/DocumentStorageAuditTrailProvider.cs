using EnsembleFX.StorageAdapter.Abstractions;
using EnsembleFX.StorageAdapter.Audit.Abstractions;
using EnsembleFX.StorageAdapter.Model.Audit.Abstractions;
using EnsembleFX.StorageAdapter.Model.DocumentStore;
using System.Threading.Tasks;

namespace EnsembleFX.StorageAdapter.Audit.DocumentStore
{
    /// <summary>
    /// Class DocumentStorageAuditTrailProvider implementing IAuditTrailProvider interface to save audit to the database and query audit from the database.
    /// </summary>
    public class DocumentStorageAuditTrailProvider : IAuditTrailProvider
	{
		#region Private members
		private IDocumentStorageAdapter<DocumentStoreAuditTrailLog> _dsStorageAdapter;        
		#endregion

		#region Constructors
		/// <summary>
		/// Gets a new instance of <see cref="DocumentManager"/> class.
		/// </summary>
		/// <param name="documentManager">Used to call DocumentManager's methods</param>
		public DocumentStorageAuditTrailProvider(IDocumentStorageAdapter<DocumentStoreAuditTrailLog> dsStorageAdapter)
		{
			_dsStorageAdapter = dsStorageAdapter;
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
				 await _dsStorageAdapter.AddAsync((DocumentStoreAuditTrailLog)auditTrailLog);
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
				//var result = await _dsStorageAdapter.GetAllAsync();
				/*
				return await _dsStorageAdapter.QueryAudit(auditTrailPaging.Start, auditTrailPaging.End);
				*/
			}
			return null;
		}
		#endregion
	}
}
