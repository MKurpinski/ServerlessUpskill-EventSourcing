using System.Threading.Tasks;
using Upskill.Events;
using Upskill.EventStore;
using Upskill.Results;

namespace Application.EventStore.Facades
{
    public class EventStoreFacade : IEventStoreFacade
    {
        private readonly IEventStore<Aggregates.Application> _eventStore;

        public EventStoreFacade(IEventStore<Aggregates.Application> eventStore)
        {
            _eventStore = eventStore;
        }

        public Task<IMessageResult> AppendEvent(string streamId, IEvent @event)
        {
            return _eventStore.AppendEvent(streamId, @event);
        }
    }
}