using Streamstone;
using Upskill.Events;
using Upskill.EventStore.Models;

namespace Upskill.EventStore.Builder
{
    public interface IEventDataBuilder
    {
        EventData BuildEventData<T>(IEvent eventData) where T: IAggregate;
    }
}
