using EnsembleFX.Messaging.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public class WorkflowTaskCommandMessage
    {
        public WorkflowTaskCommandMessage()
        {
            CommandID = Guid.NewGuid();
            CreatedOn = DateTime.UtcNow;
            this.WebHooks = new List<string>();
        }

        public Guid CommandID { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid WorkflowInstanceId { get; set; }
        public string WorkTaskId { get; set; }
        public WorkFlowTaskType TaskType { get; set; }
        public Guid ActionScriptId { get; set; }
        public Guid AgentFamilyId { get; set; }
        public Guid EnvironmentId { get; set; }
        public string BlobUrl { get; set; }
        public Guid CorelationId { get; set; }
        public Guid PackageId { get; set; }
        public List<string> WebHooks { get; set; }
        public List<SessionVariable> SessionVariables { get; set; }

    }
}
