using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public class FileTriggerMetatData : TriggerMetaData
    {
        public dynamic Content { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string FolderPath { get; set; }
        public string Size { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string Attributes { get; set; }
        public string Owner { get; set; }

    }
}
