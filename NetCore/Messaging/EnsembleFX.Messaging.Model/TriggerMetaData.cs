using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public abstract class TriggerMetaData
    {
        public int Id { get; set; }
        public Guid AppletId { get; set; }
        public Guid EnvironmentId { get; set; }
        public Guid AgentId { get; set; }
        public DateTime CreatedDate { get; set; }

        public string BlobURL { get; set; }

    }
}
