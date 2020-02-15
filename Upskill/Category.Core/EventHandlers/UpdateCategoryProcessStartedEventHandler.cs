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
using Upskill.Infrastructure.Enums;
using Upskill.Infrastructure.Extensions;

namespace Category.Core.EventHandlers
{
    public class UpdateCategoryProcessStartedEventHandler : BaseCategoryModificationHandler, IEventHandler<UpdateCategoryProcessStartedEvent>
    {
        private readonly ISearchableCategoryIndexer _categoryIndexer;
        private readonly ICategorySearchHandler _categorySearchHandler;
        private readonly ILogger<UpdateCategoryProcessStartedEventHandler> _logger;

        public UpdateCategoryProcessStartedEventHandler(
            ISearchableCategoryIndexer categoryIndexer,
            ILogger<UpdateCategoryProcessStartedEventHandler> logger,
            IEventPublisher eventPublisher,
            IEventStore<Aggregates.Category> eventStore,
            ICategorySearchHandler categorySearchHandler)
            :base(eventPublisher, eventStore)
        {
            _categoryIndexer = categoryIndexer;
            _logger = logger;
            _categorySearchHandler = categorySearchHandler;
        }

        public async Task Handle(UpdateCategoryProcessStartedEvent categoryChangedEvent)
        {
            var canSave = await this.SaveGuard(categoryChangedEvent);

            if (!canSave)
            {
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
                var failedEvent = new UpdatingCategoryFailedEvent(categoryChangedEvent.Id, CategoryModificationStatus.UnexpectedProblem, categoryChangedEvent.CorrelationId);
                await this.SaveAndDispatchEvent(categoryChangedEvent.Id, failedEvent);
                _logger.LogProgress(OperationStatus.Failed, "Problem occured while saving the category", categoryChangedEvent.CorrelationId);
                return;
            }

            var successEvent = this.GetSuccessEvent(categoryChangedEvent);
            _logger.LogProgress(OperationStatus.Finished, string.Empty, categoryChangedEvent.CorrelationId);
            await this.SaveAndDispatchEvent(categoryChangedEvent.Id, successEvent);
        }

        private async Task<bool> SaveGuard(UpdateCategoryProcessStartedEvent categoryChangedEvent)
        {
            var existingCategoryResult = await _categorySearchHandler.GetById(new GetCategoryByIdQuery(categoryChangedEvent.Id));

            if (!existingCategoryResult.Success)
            {
                var failedEvent =
                    new UpdatingCategoryFailedEvent(categoryChangedEvent.Id, CategoryModificationStatus.NotFound, categoryChangedEvent.CorrelationId);
                await this.SaveAndDispatchEvent(categoryChangedEvent.Id, failedEvent);
                _logger.LogProgress(OperationStatus.Failed, $"Category({categoryChangedEvent.Id}) cannot be found", categoryChangedEvent.CorrelationId);
                return false;
            }

            var existingCategoryWithSameNameResult = await _categorySearchHandler.GetByName(new GetCategoryByNameQuery(categoryChangedEvent.Name));

            if (existingCategoryWithSameNameResult.Success)
            {
                var failedEvent = new UpdatingCategoryFailedEvent(categoryChangedEvent.Id, CategoryModificationStatus.DuplicatedName,
                    categoryChangedEvent.CorrelationId);
                await this.SaveAndDispatchEvent(categoryChangedEvent.Id, failedEvent);
                _logger.LogProgress(OperationStatus.Failed, $"Cannot save category({categoryChangedEvent.Id}). Duplicated name", categoryChangedEvent.CorrelationId);
                return false;
            }

            return true;
        }

        private CategoryUpdatedEvent GetSuccessEvent(UpdateCategoryProcessStartedEvent changedEvent)
        {       return new CategoryUpdatedEvent(
                changedEvent.Id,
                changedEvent.Name,
                changedEvent.Description,
                changedEvent.SortOrder,
                changedEvent.CorrelationId);
        }
    }
}
