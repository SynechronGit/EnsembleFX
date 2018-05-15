using System.Collections.Generic;

namespace EnsembleFX.StorageAdapter.Model.Audit.Abstractions
{
    public interface IAuditTrail
    {
        string DateTimeStamp { get; set; }
        AuditAction AuditActionType { get; set; }
        List<IAuditDelta> Changes { get; set; }
    }
}
