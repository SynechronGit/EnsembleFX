using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public class ConfigurationVariable : IDocument
    {
        public Guid _id { get; set; }

        public string WorkSpaceID { get; set; }

        public Guid WorkflowId { get; set; }

        public Guid WorkflowDefinitionId { get; set; }

        public Guid EntityId { get; set; }

        public string Name { get; set; }

        public string Project { get; set; }

        public bool IsCollection { get; set; }

        public string VariableType { get; set; }

        public string ValueString { get; set; }

        public object ValueObject { get; set; }

        public string Description { get; set; }
        public Guid DomainId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
