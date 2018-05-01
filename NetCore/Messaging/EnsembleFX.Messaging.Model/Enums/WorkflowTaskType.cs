using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model.Enums
{
    public enum WorkFlowTaskType : int
    {
        Unknown = 0,
        Start = 1,
        Condition = 2,
        LoopStart = 3,
        LoopEnd = 4,
        ManualTask = 5,
        Action_TestComplete = 6,
        Action_PythonScript = 7,
        Action_VBScript = 8,
        Action_Workflow = 9,
        Error = 10
    }
}
