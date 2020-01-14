using System.Threading.Tasks;
using Application.Category.Events.Incoming;
using Application.Storage.Dtos;
using Application.Storage.Tables.Repositories;
using Microsoft.Extensions.Logging;
using Upskill.Events;

namespace Application.Category.EventHandlers
{
    public class CategoryCreatedEventHandler : IEventHandler<CategoryCreatedEvent>
    {
        private readonly ILogger<CategoryCreatedEvent> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryCreatedEventHandler(
            ILogger<CategoryCreatedEvent> logger,
            ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(CategoryCreatedEvent categoryCreatedEvent)
        {
            await _categoryRepository.CreateOrUpdate(new CategoryDto(categoryCreatedEvent.Id, categoryCreatedEvent.Name));
            _logger.LogInformation($"{nameof(CategoryUpdatedEventHandler)}: category: {categoryCreatedEvent.Id}");
        }
    }
}
