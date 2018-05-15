using EnsembleFX.StorageAdapter.Model.Audit.Abstractions;
using System.Threading.Tasks;

namespace EnsembleFX.StorageAdapter.Audit.Abstractions
{
    /// <summary>
    /// Interface declaring methods to save audit to the database and query audit from the database.
    /// </summary>
    public interface IAuditTrailProvider
    {
		/// <summary>
		/// Inserts AuditLog into the database
		/// </summary>
		/// <param name="auditTrailLog"></param>
		/// <returns> <c>True</c> if log is saved succesfully into the database; otherwise, <c>false</c></returns>
		Task<bool> AddLogAsync(IAuditTrailLog auditTrailLog);
	
		/// <summary>
		/// Queries AuditLog in the database
		/// </summary>
		/// <param name="auditTrailPaging"></param>
		/// <returns>Query result defined by AuditPaging range</returns>
		Task<IAuditTrailLog> QueryAuditLogsAsync(IAuditTrailPaging auditTrailPaging);
    }
}
