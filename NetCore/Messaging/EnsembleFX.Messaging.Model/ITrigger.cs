using EnsembleFX.Messaging.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public interface ITrigger
    {
        Guid AgentFamilyId { get; set; }
        Status Status { get; set; }
        string TriggerName { get; set; }
        Guid WorkspaceID { get; set; }
        Guid _id { get; set; }
        DateTime? ExpiryDate { get; set; }
        DateTime? StartDateTime { get; set; }
    }
}
