using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public class Setting : IDocument
    {
        public Guid _id { get; set; }

        public ScriptDetails scriptName { get; set; }

        public HumanAction selectedhumanaction { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
