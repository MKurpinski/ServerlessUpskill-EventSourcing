using System.Threading.Tasks;
using Upskill.Events;

namespace Upskill.EventsInfrastructure.Publishers
{
    public interface IEventPublisher
    {
        Task PublishEvent<T>(T eventContent) where T : IEvent;
    }
}
