using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public class WorkflowStep
    {
        public string WorkflowID { get; set; }

        public string WorkflowName { get; set; }

        public string WorkflowStepID { get; set; }

        public string SequenceNumber { get; set; }

        public bool ShouldExecute { get; set; }

        public string WorkflowActivityID { get; set; }

        public string TestCaseSource { get; set; }

        public string DDT { get; set; }

        public string DDTFilter { get; set; }

        public string DDTSortBy { get; set; }

        public string ShouldExitOnFailure { get; set; }

    }
}
