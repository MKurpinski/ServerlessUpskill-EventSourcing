using System.Threading.Tasks;
using Category.EventStore.Facades;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;

namespace Category.Core.EventHandlers
{
    public abstract class BaseCategoryModificationHandler
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStoreFacade _eventStore;

        protected BaseCategoryModificationHandler(
            IEventPublisher eventPublisher,
            IEventStoreFacade eventStore)
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
