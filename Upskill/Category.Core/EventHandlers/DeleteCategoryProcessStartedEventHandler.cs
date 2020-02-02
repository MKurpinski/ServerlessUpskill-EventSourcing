using System.Threading.Tasks;
using Category.Core.Enums;
using Category.Core.Events;
using Category.Core.Validators;
using Category.Search.Indexers;
using Microsoft.Extensions.Logging;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.EventStore;

namespace Category.Core.EventHandlers
{
    public class DeleteCategoryProcessStartedEventHandler : BaseCategoryModificationHandler, IEventHandler<DeleteCategoryProcessStartedEvent>
    {
        private readonly ISearchableCategoryIndexer _categoryIndexer;
        private readonly IDeleteValidator _deleteValidator;
        private readonly ILogger<DeleteCategoryProcessStartedEventHandler> _logger;

        public DeleteCategoryProcessStartedEventHandler(
            ISearchableCategoryIndexer categoryIndexer,
            ILogger<DeleteCategoryProcessStartedEventHandler> logger,
            IDeleteValidator deleteValidator, 
            IEventPublisher eventPublisher,
            IEventStore<Aggregates.Category> eventStore)
            :base(eventPublisher, eventStore)
        {
            _categoryIndexer = categoryIndexer;
            _logger = logger;
            _deleteValidator = deleteValidator;
        }

        public async Task Handle(DeleteCategoryProcessStartedEvent categoryDeletedEvent)
        {
            var canDeleteResult = await _deleteValidator.CanDelete(categoryDeletedEvent.Id);

            if (!canDeleteResult.Success)
            {
                var failedEvent = new DeletingCategoryFailedEvent(categoryDeletedEvent.Id, canDeleteResult.Status, categoryDeletedEvent.CorrelationId);
                await this.SaveAndDispatchEvent(categoryDeletedEvent.Id, failedEvent);
                _logger.LogError($"Cannot delete the category({categoryDeletedEvent.Id}), it's used somewhere");
                return;
            }

            var deleteResult = await _categoryIndexer.Delete(categoryDeletedEvent.Id);
            if (!deleteResult.Success)
            {
                var failedEvent = new DeletingCategoryFailedEvent(categoryDeletedEvent.Id, CategoryModificationStatus.UnexpectedProblem, categoryDeletedEvent.CorrelationId);
                await this.SaveAndDispatchEvent(categoryDeletedEvent.Id, failedEvent);
                _logger.LogError($"Problem occured while deleting the category: {categoryDeletedEvent.Id}");
            }

            var successEvent = new CategoryDeletedEvent(categoryDeletedEvent.Id, categoryDeletedEvent.CorrelationId);
            await this.SaveAndDispatchEvent(categoryDeletedEvent.Id, successEvent);
        }
    }
}
