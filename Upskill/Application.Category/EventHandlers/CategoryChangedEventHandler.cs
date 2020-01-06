using System.Threading.Tasks;
using Application.Category.Events.Incoming;
using Application.Core.Handlers;
using Application.Storage.Dtos;
using Application.Storage.Tables.Repositories;
using Microsoft.Extensions.Logging;
using Upskill.Events;

namespace Application.Category.EventHandlers
{
    public class CategoryChangedEventHandler: IEventHandler<CategoryChangedEvent>
    {
        private readonly ILogger<CategoryChangedEventHandler> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryNameChangedHandler _categoryNameChangedHandler;

        public CategoryChangedEventHandler(
            ILogger<CategoryChangedEventHandler> logger,
            ICategoryRepository categoryRepository,
            ICategoryNameChangedHandler categoryNameChangedHandler)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _categoryNameChangedHandler = categoryNameChangedHandler;
        }

        public async Task Handle(CategoryChangedEvent categoryChangedEvent)
        {
            var existingCategoryResult = await _categoryRepository.GetById(categoryChangedEvent.Id);

            if (existingCategoryResult.Success && !existingCategoryResult.Value.Name.Equals(categoryChangedEvent.Name))
            {
                await _categoryNameChangedHandler.HandleCategoryNameChange(
                    existingCategoryResult.Value.Name,
                    categoryChangedEvent.Name);
            }

            await _categoryRepository.CreateOrUpdate(new CategoryDto(categoryChangedEvent.Id, categoryChangedEvent.Name));

            _logger.LogInformation($"{nameof(CategoryChangedEventHandler)}: category: {categoryChangedEvent.Id}");
        }
    }
}
