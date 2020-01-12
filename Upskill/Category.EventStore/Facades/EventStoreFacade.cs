using System.Threading.Tasks;
using Upskill.Events;
using Upskill.EventStore;
using Upskill.Results;

namespace Category.EventStore.Facades
{
    public class EventStoreFacade : IEventStoreFacade
    {
        private readonly IEventStore _eventStore;

        public EventStoreFacade(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public Task<IMessageResult> AppendEvent(string streamId, IEvent @event)
        {
            return _eventStore.AppendEvent<Aggregates.Category>(streamId, @event);
        }
    }
}