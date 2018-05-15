using EnsembleFX.StorageAdapter.Model.Audit;
using System.Threading.Tasks;

namespace EnsembleFX.StorageAdapter.Audit
{
    public interface IAuditProvider<in T>
    {
        Task AddAuditLogAsync(AuditAction action ,string id , T before, T after);
    }
}
