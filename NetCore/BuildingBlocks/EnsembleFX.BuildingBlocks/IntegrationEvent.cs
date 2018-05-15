using System;

namespace EnsembleFX.BuildingBlocks
{
	public class IntegrationEvent
	{
		public IntegrationEvent()
		{
			Id = Guid.NewGuid();
			CreationDate = DateTime.UtcNow;
		}

		Guid Id { get; set; }
		DateTime CreationDate { get; set; }
	}
}
