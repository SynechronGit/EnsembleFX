namespace EnsembleFX.StorageAdapter.Model.Audit
{
    /// <summary>
    /// Represents Audit Delta values 
    /// </summary>
    public interface IAuditDelta
    {
        /// <summary>
        /// Field or Property Name
        /// </summary>
        string FieldName { get; set; }
        /// <summary>
        /// Updates Field or Property Value
        /// </summary>
        string Value { get; set; }
    }
}
