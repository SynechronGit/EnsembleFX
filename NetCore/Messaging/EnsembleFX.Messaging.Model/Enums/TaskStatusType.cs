using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model.Enums
{
    public enum TaskStatusType : int
    {
        Unknown = 0,
        Created = 1,
        Queued = 2,
        Dequeued = 3,
        Executing = 4,
        Aborted = 5,
        Completed = 6,
        CompletedWithSuccess = 7,
        CompletedWithFailure = 8,
        CompletedPendingRetry = 9

    }
}
