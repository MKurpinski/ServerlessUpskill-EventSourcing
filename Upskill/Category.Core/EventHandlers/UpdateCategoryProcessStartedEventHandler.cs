using System.Threading.Tasks;
using Category.Core.Enums;
using Category.Core.Events.External;
using Category.Core.Events.Internal;
using Category.EventStore.Facades;
using Category.Storage.Tables.Dtos;
using Category.Storage.Tables.Repositories;
using Microsoft.Extensions.Logging;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;

namespace Category.Core.EventHandlers
{
    public class UpdateCategoryProcessStartedEventHandler : BaseCategoryModificationHandler, IEventHandler<UpdateCategoryProcessStartedEvent>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<UpdateCategoryProcessStartedEventHandler> _logger;

        public UpdateCategoryProcessStartedEventHandler(
            ICategoryRepository categoryRepository,
            ILogger<UpdateCategoryProcessStartedEventHandler> logger,
            IEventPublisher eventPublisher,
            IEventStoreFacade eventStore)
            :base(eventPublisher, eventStore)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
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

            var saveResult = await _categoryRepository.CreateOrUpdate(category);

            if (!saveResult.Success)
            {
                var failedEvent = new UpdatingCategoryFailedEvent(CategoryModificationStatus.UnexpectedProblem, categoryChangedEvent.CorrelationId);
                await this.SaveAndDispatchEvent(categoryChangedEvent.Id, failedEvent);
                _logger.LogError($"Problem occured while saving the category: {categoryChangedEvent.Id}");
                return;
            }

            var successEvent = this.GetSuccessEvent(categoryChangedEvent);
            await this.SaveAndDispatchEvent(categoryChangedEvent.Id, successEvent);
        }

        private async Task<bool> SaveGuard(UpdateCategoryProcessStartedEvent categoryChangedEvent)
        {
            var existingCategoryResult = await _categoryRepository.GetById(categoryChangedEvent.Id);

            if (!existingCategoryResult.Success)
            {
                var failedEvent =
                    new UpdatingCategoryFailedEvent(CategoryModificationStatus.NotFound, categoryChangedEvent.CorrelationId);
                await this.SaveAndDispatchEvent(categoryChangedEvent.Id, failedEvent);
                _logger.LogError($"Cannot save the category: {categoryChangedEvent.Id}");
                return false;
            }

            var existingCategoryWithSameNameResult = await _categoryRepository.GetByName(categoryChangedEvent.Name);

            if (existingCategoryWithSameNameResult.Success)
            {
                var failedEvent = new UpdatingCategoryFailedEvent(CategoryModificationStatus.DuplicatedName,
                    categoryChangedEvent.CorrelationId);
                await this.SaveAndDispatchEvent(categoryChangedEvent.Id, failedEvent);
                _logger.LogError($"Cannot save the category: {categoryChangedEvent.Id}");
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
