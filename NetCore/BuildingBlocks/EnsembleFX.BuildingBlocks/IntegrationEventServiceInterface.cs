using System.Threading.Tasks;

namespace EnsembleFX.BuildingBlocks
{
	public interface IntegrationEventServiceInterface
	{
		Task PublishThroughEventBusAsync(IntegrationEvent evt);
	}
}
