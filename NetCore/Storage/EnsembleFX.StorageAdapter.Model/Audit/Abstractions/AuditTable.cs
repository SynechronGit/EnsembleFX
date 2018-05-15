using System;
using System.ComponentModel.DataAnnotations;

namespace EnsembleFX.StorageAdapter.Model.Audit.Abstractions
{
    public class AuditTable
    {
        [Key]
        public Guid _id { get; set; }
        public string EntityName { get; set; }
        public string  EntityId { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string Changes { get; set; }
        public int AuditActionType { get; set; }
    }
}
