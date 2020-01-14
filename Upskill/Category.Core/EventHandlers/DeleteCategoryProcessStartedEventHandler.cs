using System.Threading.Tasks;
using Category.Core.Events.External;
using Category.Core.Events.Internal;
using Category.Core.Validators;
using Category.EventStore.Facades;
using Category.Storage.Tables.Repositories;
using Microsoft.Extensions.Logging;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.Infrastructure.Extensions;

namespace Category.Core.EventHandlers
{
    public class DeleteCategoryProcessStartedEventHandler : IEventHandler<DeleteCategoryProcessStartedEvent>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDeleteValidator _deleteValidator;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStoreFacade _eventStore;
        private readonly ILogger<DeleteCategoryProcessStartedEventHandler> _logger;

        public DeleteCategoryProcessStartedEventHandler(
            ICategoryRepository categoryRepository,
            ILogger<DeleteCategoryProcessStartedEventHandler> logger,
            IDeleteValidator deleteValidator, 
            IEventPublisher eventPublisher,
            IEventStoreFacade eventStore)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _deleteValidator = deleteValidator;
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
        }

        public async Task Handle(DeleteCategoryProcessStartedEvent categoryDeletedEvent)
        {
            var canDelete = await _deleteValidator.CanDelete(categoryDeletedEvent.Id);

            if (!canDelete.Success)
            {
                _logger.LogError($"Cannot delete the category({categoryDeletedEvent.Id}), it's used somewhere");
                return;
            }

            var deleteResult = await _categoryRepository.Delete(categoryDeletedEvent.Id);
            if (!deleteResult.Success)
            {
                _logger.LogError($"Problem occured while deleting the category: {categoryDeletedEvent.Id}");
            }

            var successEvent = new CategoryDeletedEvent(categoryDeletedEvent.Id, categoryDeletedEvent.CorrelationId);
            var saveEventStatus = await _eventStore.AppendEvent(categoryDeletedEvent.Id, successEvent);

            if (!saveEventStatus.Success)
            {
                _logger.LogErrors("Problem occured while saving the success event", saveEventStatus.Errors);
                return;
            }

            await _eventPublisher.PublishEvent(successEvent);
        }
    }
}
