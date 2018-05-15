namespace EnsembleFX.BuildingBlocks
{
	/// <summary>
	/// This Interface represents the Domain entity type
	/// </summary>
	public interface IEntity<out TId>
	{
		/// <summary>
		///  Gets the ID which uniquely identifies the entity instance within its type's bounds.
		/// </summary>
		TId _id { get; }
		/// <summary>
		/// returns a value indicating whether the current object is transient.
		/// </summary>
		/// <returns></returns>
		bool IsTransient();
	}
}
