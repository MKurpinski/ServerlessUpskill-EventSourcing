using System.Threading.Tasks;
using Category.Core.Events.External;
using Category.Core.Events.Internal;
using Category.EventStore.Facades;
using Category.Storage.Tables.Repositories;
using Microsoft.Extensions.Logging;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;

namespace Category.Core.EventHandlers
{
    public class CreateCategoryProcessStartedEventHandler : BaseModifyCategoryProcessEventHandler<CreateCategoryProcessStartedEvent>, IEventHandler<CreateCategoryProcessStartedEvent>
    {
        public CreateCategoryProcessStartedEventHandler(
            ICategoryRepository categoryRepository,
            ILogger<CreateCategoryProcessStartedEvent> logger,
            IEventPublisher eventPublisher,
            IEventStoreFacade eventStore)
            : base(categoryRepository, logger, eventPublisher, eventStore)
        {
        }

        public async Task Handle(CreateCategoryProcessStartedEvent categoryChangedEvent)
        {
            await this.HandleInternal(categoryChangedEvent);
        }

        protected override async Task<bool> CanBeSaved(CreateCategoryProcessStartedEvent changedEvent)
        {
            var existingCategoryResult = await CategoryRepository.GetByName(changedEvent.Name);
            return !existingCategoryResult.Success;
        }

        protected override IEvent GetSuccessEvent(CreateCategoryProcessStartedEvent changedEvent)
        {
            return new CategoryCreatedEvent(
                changedEvent.Id,
                changedEvent.Name,
                changedEvent.Description,
                changedEvent.SortOrder,
                changedEvent.CorrelationId);
        }
    }
}
