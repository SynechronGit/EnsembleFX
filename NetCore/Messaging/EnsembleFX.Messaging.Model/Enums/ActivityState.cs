using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model.Enums
{
    public enum ActivityState
    {
        InProgess,
        Queued,
        Failed,
        Passed,
        Started,
        Aborted
    }
}
