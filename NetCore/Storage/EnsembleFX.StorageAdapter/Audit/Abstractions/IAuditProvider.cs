using EnsembleFX.StorageAdapter.Model.Audit.Abstractions;
using System.Threading.Tasks;

namespace EnsembleFX.StorageAdapter.Audit.Abstractions
{
    public interface IAuditProvider<in T>
    {
        Task AddAuditLogAsync(AuditAction action ,string id , T before, T after);
    }
}
