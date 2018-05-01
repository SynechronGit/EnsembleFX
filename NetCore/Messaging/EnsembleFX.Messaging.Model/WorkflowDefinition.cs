using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public class WorkflowDefinition : IDocument
    {
        public Guid _id { get; set; }
        public int WorkflowId { get; set; }
        public dynamic Definition { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        //stores the workflow definition version name
        public string Version { get; set; }

        public int PublishStatus { get; set; } //0 - Draft, 1 - Published
        public object BlobUrl { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
}
