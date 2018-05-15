using System.Threading.Tasks;

namespace EnsembleFX.BuildingBlocks
{
	public interface IEventBus
	{
		Task PublishAsync(IntegrationEvent @event);
	}
}
