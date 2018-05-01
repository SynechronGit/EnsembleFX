using EnsembleFX.Messaging.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public class WorkflowTaskStatus
    {
        public string TaskId { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsSuccessful { get; set; }
        public DateTime StartedOnUTC { get; set; }
        public DateTime CompletedOnUTC { get; set; }
        public Guid AssignedAgentFamiltyId { get; set; }
        public Guid AssignedAgentConfigurationId { get; set; }
        public string Output { get; set; }
        public WorkFlowTaskType TaskType { get; set; }
        public Guid ActionScriptId { get; set; }
        public Guid PackageId { get; set; }
        public TaskStatusType Status { get; set; }
    }
}
