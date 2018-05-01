using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public class Workflow : IDocument
    {
        public Workflow()
        {
            Steps = new List<WorkflowStep>();
            Activities = new List<WorkflowActivityNode>();
            ActivityLinkData = new List<WorkflowLinkData>();
            Definitions = new List<WorkflowDefinition>();
        }

        public Guid _id { get; set; }

        public string WorkFlowID { get; set; }

        public Guid ProjectId { get; set; }

        public string WorkSpaceID { get; set; }

        public string WorkflowName { get; set; }

        public string WorkFlowDescription { get; set; }

        public List<WorkflowStep> Steps { get; set; }

        public List<WorkflowActivityNode> Activities { get; set; }

        public List<WorkflowLinkData> ActivityLinkData { get; set; }

        public List<WorkflowDefinition> Definitions { get; set; }

        public bool Status { get; set; }

        public string ConditionExpression { get; set; }

        public decimal DollarsSaved { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
}
