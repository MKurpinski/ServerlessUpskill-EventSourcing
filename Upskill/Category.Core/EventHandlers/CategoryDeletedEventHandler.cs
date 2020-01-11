using System.Threading.Tasks;
using Category.Core.Events;
using Category.Core.Events.External;
using Category.Core.Events.Internal;
using Category.Core.Validators;
using Category.Storage.Tables.Repositories;
using Microsoft.Extensions.Logging;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;

namespace Category.Core.EventHandlers
{
    public class CategoryDeletedEventHandler : IEventHandler<InternalCategoryDeletedEvent>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDeleteValidator _deleteValidator;
        private readonly IEventPublisher _eventPublisher;

        private readonly ILogger<CategoryDeletedEventHandler> _logger;

        public CategoryDeletedEventHandler(
            ICategoryRepository categoryRepository,
            ILogger<CategoryDeletedEventHandler> logger,
            IDeleteValidator deleteValidator, 
            IEventPublisher eventPublisher)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _deleteValidator = deleteValidator;
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(InternalCategoryDeletedEvent categoryDeletedEvent)
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

            await _eventPublisher.PublishEvent(new CategoryDeletedEvent(categoryDeletedEvent.Id));
        }
    }
}
