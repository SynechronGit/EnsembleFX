using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public class AzureMessageModel
    {
        public string TriggerName { get; set; }

        public string TriggerType { get; set; }

        public string IFTTTAppletName { get; set; }

        public string WorkFlowName { get; set; }

        public string WorkFlowTaskType { get; set; }

        public string WorkFlowTaskTypeStatus { get; set; }

        public string AgentName { get; set; }

        public DateTime StartedOnUTC { get; set; }

        public string Output { get; set; }

        public string MessageType;

        public string JSONData { get; set; }

        public AzureMessageModel()
        {
            TriggerName = "";
            TriggerType = "";
            IFTTTAppletName = "";
            WorkFlowName = "";
            WorkFlowTaskType = "";
            WorkFlowTaskTypeStatus = "";
            AgentName = "";
            Output = "";
            MessageType = "";
        }
    }
}
