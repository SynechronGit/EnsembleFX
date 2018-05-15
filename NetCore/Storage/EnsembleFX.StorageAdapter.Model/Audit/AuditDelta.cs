namespace EnsembleFX.StorageAdapter.Model.Audit
{
    /// <summary>
    /// Represents Audit Delta values 
    /// </summary>
    public class AuditDelta : IAuditDelta
    {
        /// <summary>
        /// Field or Property Name
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Updates Field or Property Value
        /// </summary>
        public string Value { get; set; }
    }
}
