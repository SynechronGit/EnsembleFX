using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public class WorkflowOrchestratorMessage
    {
        public Guid WorkflowInstanceId { get; set; }
        public Guid WorkflowId { get; set; }
        public Guid WorkflowDefinitionId { get; set; }

    }
}
