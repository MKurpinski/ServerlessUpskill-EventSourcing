using System.Threading.Tasks;
using Application.Category.Events.Incoming;
using Application.Category.Events.Outcoming;
using Application.Storage.Dtos;
using Application.Storage.Tables.Repositories;
using Microsoft.Extensions.Logging;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;

namespace Application.Category.EventHandlers
{
    public class CategoryUpdatedEventHandler: IEventHandler<CategoryUpdatedEvent>
    {
        private readonly ILogger<CategoryUpdatedEventHandler> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEventPublisher _eventPublisher;

        public CategoryUpdatedEventHandler(
            ILogger<CategoryUpdatedEventHandler> logger,
            ICategoryRepository categoryRepository,
            IEventPublisher eventPublisher)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(CategoryUpdatedEvent categoryUpdatedEvent)
        {
            var existingCategoryResult = await _categoryRepository.GetById(categoryUpdatedEvent.Id);

            if (existingCategoryResult.Success && !existingCategoryResult.Value.Name.Equals(categoryUpdatedEvent.Name))
            {

                await _eventPublisher.PublishEvent(
                    new CategoryNameChangedEvent(
                        existingCategoryResult.Value.Name,
                        categoryUpdatedEvent.Name,
                        categoryUpdatedEvent.CorrelationId));
            }

            await _categoryRepository.CreateOrUpdate(new CategoryDto(categoryUpdatedEvent.Id, categoryUpdatedEvent.Name));

            _logger.LogInformation($"{nameof(CategoryUpdatedEventHandler)}: category: {categoryUpdatedEvent.Id}");
        }
    }
}
