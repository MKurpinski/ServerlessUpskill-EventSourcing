using System.Threading.Tasks;

namespace Upskill.EventPublisher.Publishers
{
    public interface IEventPublisher
    {
        Task PublishEvent<T>(T eventContent) where T : IEvent;
    }
}
