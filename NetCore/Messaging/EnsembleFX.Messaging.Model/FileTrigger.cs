using EnsembleFX.Messaging.Model.Enums;
using System;

namespace EnsembleFX.Messaging.Model
{
    public class FileTrigger : ITrigger, IDocument
    {
        public Guid _id { get; set; }

        /// <summary>
        /// WorkspaceID
        /// </summary>
        public Guid WorkspaceID { get; set; }

        /// <summary>
        /// Trigger File Path
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Trigger file filter
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Project ID
        /// </summary>
        public string ProjectID { get; set; }

        /// <summary>
        /// Is Trigger Active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Trigger Start Date
        /// </summary>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// Trigger Expiry Date
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Trigger Status
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Trigger Name
        /// </summary>
        public string TriggerName { get; set; }

        /// <summary>
        /// Agent Family ID
        /// </summary>
        public Guid AgentFamilyId { get; set; }

        // added for #18 github Issues
        public int RefreshHours { set; get; }

        public int RefreshMinutes { set; get; }

        public int RefreshSeconds { set; get; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
