using System.Threading.Tasks;
using Category.Core.Enums;
using Category.Core.Events;
using Category.Core.Validators;
using Category.Search.Indexers;
using Microsoft.Extensions.Logging;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.EventStore;
using Upskill.Infrastructure.Enums;
using Upskill.Infrastructure.Extensions;

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
                _logger.LogProgress(OperationStatus.Failed, "Category is used", categoryDeletedEvent.CorrelationId);
                return;
            }

            var deleteResult = await _categoryIndexer.Delete(categoryDeletedEvent.Id);
            if (!deleteResult.Success)
            {
                var failedEvent = new DeletingCategoryFailedEvent(categoryDeletedEvent.Id, CategoryModificationStatus.UnexpectedProblem, categoryDeletedEvent.CorrelationId);
                await this.SaveAndDispatchEvent(categoryDeletedEvent.Id, failedEvent);
                _logger.LogProgress(OperationStatus.Failed, $"Problem occured during saving category {categoryDeletedEvent.Id}", categoryDeletedEvent.CorrelationId);
            }

            var successEvent = new CategoryDeletedEvent(categoryDeletedEvent.Id, categoryDeletedEvent.CorrelationId);
            _logger.LogProgress(OperationStatus.Finished, string.Empty, categoryDeletedEvent.CorrelationId);
            await this.SaveAndDispatchEvent(categoryDeletedEvent.Id, successEvent);
        }
    }
}
