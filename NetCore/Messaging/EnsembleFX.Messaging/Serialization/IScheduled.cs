
namespace EnsembleFX.Messaging.Serialization
{
    public interface IScheduled
    {
        void Execute(IMessage schedularMessage);
    }
}
