using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public class WorkflowOrchestratorMessageEnvelope
    {
        public TriggerEventMessage TriggerEventMessage { get; set; }
        public WorkflowTaskStatusMessage WorkflowTaskStatusMessage { get; set; }
    }
}
