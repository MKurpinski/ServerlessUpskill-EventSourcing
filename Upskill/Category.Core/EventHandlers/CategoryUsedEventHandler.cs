using System.Threading.Tasks;
using Category.Core.Events;
using Category.DataStorage.Repositories;
using Category.Storage.Tables.Repositories;
using Microsoft.Extensions.Logging;
using Upskill.Events;

namespace Category.Core.EventHandlers
{
    public class CategoryUsedEventHandler : IEventHandler<CategoryUsedEvent>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUsedCategoryRepository _usedCategoryRepository;
        private readonly ILogger<CategoryUsedEventHandler> _logger;

        public CategoryUsedEventHandler(
            ICategoryRepository categoryRepository,
            IUsedCategoryRepository usedCategoryRepository,
            ILogger<CategoryUsedEventHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _usedCategoryRepository = usedCategoryRepository;
            _logger = logger;
        }

        public async Task Handle(CategoryUsedEvent categoryUsedEvent)
        {
            _logger.LogInformation($"{nameof(CategoryUsedEvent)} with name: {categoryUsedEvent.Name}, has been used in {categoryUsedEvent.UsedIn}");

            var categoryResult = await _categoryRepository.GetByName(categoryUsedEvent.Name);

            if (!categoryResult.Success)
            {
                _logger.LogError($"{nameof(CategoryUsedEvent)} with name: {categoryUsedEvent.Name} cannot be find. Inconsistency of data!");
                return;
            }

            await _usedCategoryRepository.CreateOrUpdate(categoryResult.Value.Id, categoryUsedEvent.UsedIn);
        }
    }
}
