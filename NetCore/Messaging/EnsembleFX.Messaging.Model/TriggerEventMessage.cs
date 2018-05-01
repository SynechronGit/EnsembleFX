using EnsembleFX.Messaging.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public class TriggerEventMessage : WorkflowOrchestratorMessage
    {

        public TriggerEventMessage()
        { this.WebHooks = new List<string>(); }

        public Guid TriggerId { get; set; }
        public TriggerType TriggerType { get; set; }
        public Guid IFTTTAppletId { get; set; }
        public EmailTriggerMetaData EmailMetadata { get; set; }
        public FileTriggerMetatData FileMetadata { get; set; }

        public string BlobUrl { get; set; }
        public Guid BlobId { get; set; }
        public Guid CorelationId { get; set; }
        public List<string> WebHooks { get; set; }
        public List<SessionVariable> SessionVariables { get; set; }
        public Guid KnowledgeBaseID { get; set; }
        public Guid KnowledgeBaseFolderID { get; set; }
    }
}
