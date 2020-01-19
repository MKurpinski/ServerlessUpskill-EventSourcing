using System.Threading.Tasks;
using Category.Core.Enums;
using Category.Core.Events.External;
using Category.Core.Events.Internal;
using Category.Core.Validators;
using Category.EventStore.Facades;
using Category.Storage.Tables.Repositories;
using Microsoft.Extensions.Logging;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;

namespace Category.Core.EventHandlers
{
    public class DeleteCategoryProcessStartedEventHandler : BaseCategoryModificationHandler, IEventHandler<DeleteCategoryProcessStartedEvent>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDeleteValidator _deleteValidator;
        private readonly ILogger<DeleteCategoryProcessStartedEventHandler> _logger;

        public DeleteCategoryProcessStartedEventHandler(
            ICategoryRepository categoryRepository,
            ILogger<DeleteCategoryProcessStartedEventHandler> logger,
            IDeleteValidator deleteValidator, 
            IEventPublisher eventPublisher,
            IEventStoreFacade eventStore)
            :base(eventPublisher, eventStore)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _deleteValidator = deleteValidator;
        }

        public async Task Handle(DeleteCategoryProcessStartedEvent categoryDeletedEvent)
        {
            var canDeleteResult = await _deleteValidator.CanDelete(categoryDeletedEvent.Id);

            if (!canDeleteResult.Success)
            {
                var failedEvent = new DeletingCategoryFailedEvent(canDeleteResult.Status, categoryDeletedEvent.CorrelationId);
                await this.SaveAndDispatchEvent(categoryDeletedEvent.Id, failedEvent);
                _logger.LogError($"Cannot delete the category({categoryDeletedEvent.Id}), it's used somewhere");
                return;
            }

            var deleteResult = await _categoryRepository.Delete(categoryDeletedEvent.Id);
            if (!deleteResult.Success)
            {
                var failedEvent = new DeletingCategoryFailedEvent(CategoryModificationStatus.UnexpectedProblem, categoryDeletedEvent.CorrelationId);
                await this.SaveAndDispatchEvent(categoryDeletedEvent.Id, failedEvent);
                _logger.LogError($"Problem occured while deleting the category: {categoryDeletedEvent.Id}");
            }

            var successEvent = new CategoryDeletedEvent(categoryDeletedEvent.Id, categoryDeletedEvent.CorrelationId);
            await this.SaveAndDispatchEvent(categoryDeletedEvent.Id, successEvent);
        }
    }
}
