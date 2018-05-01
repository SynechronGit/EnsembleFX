using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public class WorkflowLinkData
    {
        public Guid _id { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string fromport { get; set; }
        public string toport { get; set; }
        public bool visible { get; set; }
        //public float[] points { get; set; }
        public double[] points { get; set; }

    }
}
