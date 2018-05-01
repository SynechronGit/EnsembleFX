using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public class EmailTriggerMetaData : TriggerMetaData
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }

        public byte[] EmailMimeContent { get; set; }
        public Guid EmailBlobId { get; set; }


    }
}
