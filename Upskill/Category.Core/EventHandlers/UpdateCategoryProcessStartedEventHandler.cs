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
    public class UpdateCategoryProcessStartedEventHandler : BaseModifyCategoryProcessEventHandler<UpdateCategoryProcessStartedEvent>, IEventHandler<UpdateCategoryProcessStartedEvent>
    {
        public UpdateCategoryProcessStartedEventHandler(
            ICategoryRepository categoryRepository,
            ILogger<UpdateCategoryProcessStartedEvent> logger,
            IEventPublisher eventPublisher,
            IEventStoreFacade eventStore) 
            : base(categoryRepository, logger, eventPublisher, eventStore)
        {
        }

        public async Task Handle(UpdateCategoryProcessStartedEvent categoryChangedEvent)
        {
            await this.HandleInternal(categoryChangedEvent);
        }

        protected override async Task<bool> CanBeSaved(UpdateCategoryProcessStartedEvent changedEvent)
        {
            var existingCategoryResult = await CategoryRepository.GetById(changedEvent.Id);
            if (!existingCategoryResult.Success)
            {
                return false;
            }

            var existingCategoryWithSameNameResult = await CategoryRepository.GetByName(changedEvent.Name);
            return !existingCategoryWithSameNameResult.Success || (existingCategoryWithSameNameResult.Value.Id == changedEvent.Id);
        }

        protected override IEvent GetSuccessEvent(UpdateCategoryProcessStartedEvent changedEvent)
        {       return new CategoryUpdatedEvent(
                changedEvent.Id,
                changedEvent.Name,
                changedEvent.Description,
                changedEvent.SortOrder,
                changedEvent.CorrelationId);
        }
    }
}
