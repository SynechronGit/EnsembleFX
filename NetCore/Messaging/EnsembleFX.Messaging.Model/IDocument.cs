using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    /// <summary>
    /// Use for concurrency management
    /// </summary>
    public interface IDocument
    {
        string CreatedBy { get; set; }
        DateTime? CreatedOn { get; set; }
        string UpdatedBy { get; set; }
        DateTime? UpdatedOn { get; set; }
    }
}
