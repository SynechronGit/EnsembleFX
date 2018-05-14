using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Core.Helpers
{
    public partial class EmailAttachment
    {
        public string AttachmentName { get; set; }

        public MemoryStream AttachmentStream { get; set; }

        public string ContentType { get; set; }
    }
}
