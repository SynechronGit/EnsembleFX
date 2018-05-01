using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Messaging.Model
{
    public class IFTTTApplet : IDocument
    {

        /// <summary>
        /// Applet Id
        /// </summary>
        public Guid _id { get; set; }

        /// <summary>
        /// Applet Name
        /// </summary>
        public string AppletTitle { get; set; }

        /// <summary>
        /// Applet Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Trigger Type i.e File, Email or Time trigger
        /// </summary>
        public TriggerType TriggerType { get; set; }

        /// <summary>
        /// Trigger for which this applet is associated with
        /// </summary>
        public Guid TriggerId { get; set; }

        /// <summary>
        /// Workflow
        /// </summary>
        public Guid WorkflowId { get; set; }

        /// <summary>
        /// Published WorkflowDefinition/Version under selected Workflow 
        /// </summary>
        public Guid WorkflowDefinitionId { get; set; }

        /// <summary>
        /// Turn Applet On/Off
        /// </summary>
        public bool AppletOnOff { get; set; }

        public List<string> PromotedEnvironments { get; set; }

        public List<Guid> Groups { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
