using System.Threading.Tasks;
using Category.Core.Events;
using Category.Core.Events.External;
using Category.Core.Events.Internal;
using Category.Storage.Tables.Repositories;
using Microsoft.Extensions.Logging;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;

namespace Category.Core.EventHandlers
{
    public class CategoryAddedEventHandler : BaseCategoryChangedEventHandler, IEventHandler<InternalCategoryAddedEvent>
    {
        public CategoryAddedEventHandler(
            ICategoryRepository categoryRepository,
            ILogger<BaseCategoryChangedEventHandler> logger,
            IEventPublisher eventPublisher)
            : base(categoryRepository, logger, eventPublisher)
        {
        }

        public async Task Handle(InternalCategoryAddedEvent categoryChangedEvent)
        {
            await this.HandleInternal(categoryChangedEvent);
        }

        protected override async Task<bool> CanBeSaved(InternalCategoryChangedEvent changedEvent)
        {
            var existingCategoryResult = await CategoryRepository.GetByName(changedEvent.Name);
            return !existingCategoryResult.Success;
        }

        protected override async Task DispatchChangeEvent(InternalCategoryChangedEvent changedEvent)
        {
            await this.EventPublisher.PublishEvent(new CategoryAddedEvent(
                changedEvent.Id,
                changedEvent.Name,
                changedEvent.Description,
                changedEvent.SortOrder));
        }
    }
}
