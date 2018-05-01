using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model.Enums
{
    public enum WorkflowState
    {
        Queued,
        Executing,
        Failed,
        Completed,
        Started,
        Aborted
    }
}
