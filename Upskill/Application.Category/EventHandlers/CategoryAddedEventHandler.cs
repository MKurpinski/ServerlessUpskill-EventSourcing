using System.Threading.Tasks;
using Application.Category.Events.Incoming;
using Application.Storage.Dtos;
using Application.Storage.Tables.Repositories;
using Microsoft.Extensions.Logging;
using Upskill.Events;

namespace Application.Category.EventHandlers
{
    public class CategoryAddedEventHandler : IEventHandler<CategoryAddedEvent>
    {
        private readonly ILogger<CategoryChangedEventHandler> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryAddedEventHandler(
            ILogger<CategoryChangedEventHandler> logger,
            ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(CategoryAddedEvent categoryChangedEvent)
        {
            await _categoryRepository.CreateOrUpdate(new CategoryDto(categoryChangedEvent.Id, categoryChangedEvent.Name));
            _logger.LogInformation($"{nameof(CategoryChangedEventHandler)}: category: {categoryChangedEvent.Id}");
        }
    }
}
