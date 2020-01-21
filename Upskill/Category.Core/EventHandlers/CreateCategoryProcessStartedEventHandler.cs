using System.Threading.Tasks;
using Category.Core.Enums;
using Category.Core.Events.External;
using Category.Core.Events.Internal;
using Category.Storage.Tables.Dtos;
using Category.Storage.Tables.Repositories;
using Microsoft.Extensions.Logging;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.EventStore;

namespace Category.Core.EventHandlers
{
    public class CreateCategoryProcessStartedEventHandler : BaseCategoryModificationHandler, IEventHandler<CreateCategoryProcessStartedEvent>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CreateCategoryProcessStartedEventHandler> _logger;

        public CreateCategoryProcessStartedEventHandler(
            ICategoryRepository categoryRepository,
            ILogger<CreateCategoryProcessStartedEventHandler> logger,
            IEventPublisher eventPublisher,
            IEventStore<Aggregates.Category> eventStore)
            :base(eventPublisher, eventStore)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task Handle(CreateCategoryProcessStartedEvent categoryChangedEvent)
        {
            var existingCategoryWithName = await _categoryRepository.GetByName(categoryChangedEvent.Name);

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

            var saveResult = await _categoryRepository.CreateOrUpdate(category);

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
