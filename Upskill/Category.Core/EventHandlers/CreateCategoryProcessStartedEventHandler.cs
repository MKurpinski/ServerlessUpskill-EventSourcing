using System.Threading.Tasks;
using Category.Core.Enums;
using Category.Core.Events;
using Category.Search.Dtos;
using Category.Search.Handlers;
using Category.Search.Indexers;
using Category.Search.Queries;
using Microsoft.Extensions.Logging;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.EventStore;

namespace Category.Core.EventHandlers
{
    public class CreateCategoryProcessStartedEventHandler : BaseCategoryModificationHandler, IEventHandler<CreateCategoryProcessStartedEvent>
    {
        private readonly ISearchableCategoryIndexer _categoryIndexer;
        private readonly ICategorySearchHandler _categorySearchHandler;
        private readonly ILogger<CreateCategoryProcessStartedEventHandler> _logger;

        public CreateCategoryProcessStartedEventHandler(
            ISearchableCategoryIndexer categoryIndexer,
            ILogger<CreateCategoryProcessStartedEventHandler> logger,
            IEventPublisher eventPublisher,
            IEventStore<Aggregates.Category> eventStore,
            ICategorySearchHandler categorySearchHandler)
            :base(eventPublisher, eventStore)
        {
            _categoryIndexer = categoryIndexer;
            _logger = logger;
            _categorySearchHandler = categorySearchHandler;
        }

        public async Task Handle(CreateCategoryProcessStartedEvent categoryChangedEvent)
        {
            var existingCategoryWithName = await _categorySearchHandler.GetByName(new GetCategoryByNameQuery(categoryChangedEvent.Name));

            if (existingCategoryWithName.Success)
            {
                var failedEvent = new CreatingCategoryFailedEvent(CategoryModificationStatus.DuplicatedName, categoryChangedEvent.CorrelationId);
                await this.SaveAndDispatchEvent(categoryChangedEvent.Id, failedEvent);
                _logger.LogError($"Cannot save the category: {categoryChangedEvent.Id}");
                return;
            }

            var category = new CategoryDto(
                categoryChangedEvent.Id,
                categoryChangedEvent.Name,
                categoryChangedEvent.Description,
                categoryChangedEvent.SortOrder);

            var saveResult = await _categoryIndexer.Index(category);

            if (!saveResult.Success)
            {
                var failedEvent = new CreatingCategoryFailedEvent(CategoryModificationStatus.UnexpectedProblem, categoryChangedEvent.CorrelationId);
                await this.SaveAndDispatchEvent(categoryChangedEvent.Id, failedEvent);
                _logger.LogError($"Problem occured while saving the category: {categoryChangedEvent.Id}");
                return;
            }

            var successEvent = this.GetSuccessEvent(categoryChangedEvent);
            await this.SaveAndDispatchEvent(categoryChangedEvent.Id, successEvent);
        }

        private CategoryCreatedEvent GetSuccessEvent(CreateCategoryProcessStartedEvent changedEvent)
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
