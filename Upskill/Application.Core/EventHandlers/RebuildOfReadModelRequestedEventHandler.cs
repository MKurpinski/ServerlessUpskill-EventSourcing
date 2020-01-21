using System.Threading.Tasks;
using Application.Core.Events;
using Upskill.Events;
using Upskill.EventStore;

namespace Application.Core.EventHandlers
{
    public class RebuildOfReadModelRequestedEventHandler : IEventHandler<RebuildOfReadModelRequestedEvent>
    {
        private readonly IEventStore<Aggregates.Application> _eventStore;

        public RebuildOfReadModelRequestedEventHandler(IEventStore<Aggregates.Application> eventStore)
        {
            _eventStore = eventStore;
        }

        public Task Handle(RebuildOfReadModelRequestedEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
