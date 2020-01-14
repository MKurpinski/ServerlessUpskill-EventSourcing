using System.Threading.Tasks;
using Category.Core.Events.Internal;
using Category.EventStore.Facades;
using Category.Storage.Tables.Dtos;
using Category.Storage.Tables.Repositories;
using Microsoft.Extensions.Logging;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.Infrastructure.Extensions;

namespace Category.Core.EventHandlers
{
    public abstract class BaseModifyCategoryProcessEventHandler<T> where T : UpdateCategoryProcessStartedEvent
    {
        protected readonly ICategoryRepository CategoryRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStoreFacade _eventStore;
        private readonly ILogger<T> _logger;

        protected BaseModifyCategoryProcessEventHandler(
            ICategoryRepository categoryRepository,
            ILogger<T> logger,
            IEventPublisher eventPublisher,
            IEventStoreFacade eventStore)
        {
            CategoryRepository = categoryRepository;
            _logger = logger;
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
        }

        protected abstract Task<bool> CanBeSaved(T changedEvent);
        protected abstract IEvent GetSuccessEvent(T changedEvent);

        protected async Task HandleInternal(T changedEvent)
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

            var successEvent = this.GetSuccessEvent(changedEvent);

            var saveEventStatus = await _eventStore.AppendEvent(changedEvent.Id, successEvent);

            if (!saveEventStatus.Success)
            {
                _logger.LogErrors("Problem occured while saving the success event", saveEventStatus.Errors);
                return;
            }

            await _eventPublisher.PublishEvent(successEvent);
        }
    }
}
