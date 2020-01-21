using System.Threading.Tasks;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.EventStore;

namespace Category.Core.EventHandlers
{
    public abstract class BaseCategoryModificationHandler
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore<Aggregates.Category> _eventStore;

        protected BaseCategoryModificationHandler(
            IEventPublisher eventPublisher,
            IEventStore<Aggregates.Category> eventStore)
        {
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
        }

        protected async Task SaveAndDispatchEvent(string id, IEvent @event)
        {
            await _eventStore.AppendEvent(id, @event);
            await _eventPublisher.PublishEvent(@event);
        }
    }
}
