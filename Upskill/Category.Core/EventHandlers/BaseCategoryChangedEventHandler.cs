using System.Threading.Tasks;
using Category.Core.Events;
using Category.Core.Events.Internal;
using Category.Storage.Tables.Dtos;
using Category.Storage.Tables.Repositories;
using Microsoft.Extensions.Logging;
using Upskill.EventsInfrastructure.Publishers;

namespace Category.Core.EventHandlers
{
    public abstract class BaseCategoryChangedEventHandler
    {
        protected readonly ICategoryRepository CategoryRepository;
        protected readonly IEventPublisher EventPublisher;
        private readonly ILogger<BaseCategoryChangedEventHandler> _logger;

        protected BaseCategoryChangedEventHandler(
            ICategoryRepository categoryRepository,
            ILogger<BaseCategoryChangedEventHandler> logger,
            IEventPublisher eventPublisher)
        {
            CategoryRepository = categoryRepository;
            _logger = logger;
            EventPublisher = eventPublisher;
        }

        protected abstract Task<bool> CanBeSaved(InternalCategoryChangedEvent changedEvent);
        protected abstract Task DispatchChangeEvent(InternalCategoryChangedEvent changedEvent);

        protected async Task HandleInternal(InternalCategoryChangedEvent changedEvent)
        {
            var canBeSaved = await this.CanBeSaved(changedEvent);

            if (!canBeSaved)
            {
                _logger.LogError($"Cannot save the category: {changedEvent.Id}");
                return;
            }

            var category = new CategoryDto(
                changedEvent.Id,
                changedEvent.Name,
                changedEvent.Description,
                changedEvent.SortOrder);

            var saveResult = await CategoryRepository.CreateOrUpdate(category);

            if (!saveResult.Success)
            {
                _logger.LogError($"Problem occured while saving the category: {changedEvent.Id}");
            }

            await this.DispatchChangeEvent(changedEvent);
        }
    }
}
