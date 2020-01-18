using System.Threading.Tasks;
using Upskill.Events;
using Upskill.EventStore;
using Upskill.Results;

namespace Category.EventStore.Facades
{
    public class EventStoreFacade : IEventStoreFacade
    {
        private readonly IEventStore<Aggregates.Category> _eventStore;

        public EventStoreFacade(IEventStore<Aggregates.Category> eventStore)
        {
            _eventStore = eventStore;
        }

        public Task<IMessageResult> AppendEvent(string streamId, IEvent @event)
        {
            return _eventStore.AppendEvent(streamId, @event);
        }
    }
}