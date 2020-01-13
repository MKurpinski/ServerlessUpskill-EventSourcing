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
    public class CategoryChangedEventHandler: IEventHandler<CategoryChangedEvent>
    {
        private readonly ILogger<CategoryChangedEventHandler> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEventPublisher _eventPublisher;

        public CategoryChangedEventHandler(
            ILogger<CategoryChangedEventHandler> logger,
            ICategoryRepository categoryRepository,
            IEventPublisher eventPublisher)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(CategoryChangedEvent categoryChangedEvent)
        {
            var existingCategoryResult = await _categoryRepository.GetById(categoryChangedEvent.Id);

            if (existingCategoryResult.Success && !existingCategoryResult.Value.Name.Equals(categoryChangedEvent.Name))
            {

                await _eventPublisher.PublishEvent(
                    new CategoryNameChangedEvent(
                        existingCategoryResult.Value.Name,
                        categoryChangedEvent.Name,
                        categoryChangedEvent.CorrelationId));
            }

            await _categoryRepository.CreateOrUpdate(new CategoryDto(categoryChangedEvent.Id, categoryChangedEvent.Name));

            _logger.LogInformation($"{nameof(CategoryChangedEventHandler)}: category: {categoryChangedEvent.Id}");
        }
    }
}
