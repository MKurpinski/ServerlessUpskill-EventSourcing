using System.Threading.Tasks;
using Category.Core.Events;
using Category.Search.Handlers;
using Category.Search.Queries;
using Category.Storage.Tables.Repositories;
using Microsoft.Extensions.Logging;
using Upskill.Events;
using Upskill.Infrastructure.Enums;
using Upskill.Infrastructure.Extensions;

namespace Category.Core.EventHandlers
{
    public class CategoryUsedEventHandler : IEventHandler<CategoryUsedEvent>
    {
        private readonly ICategorySearchHandler _categorySearchHandler;
        private readonly IUsedCategoryRepository _usedCategoryRepository;
        private readonly ILogger<CategoryUsedEventHandler> _logger;

        public CategoryUsedEventHandler(
            ICategorySearchHandler categorySearchHandler,
            IUsedCategoryRepository usedCategoryRepository,
            ILogger<CategoryUsedEventHandler> logger)
        {
            _categorySearchHandler = categorySearchHandler;
            _usedCategoryRepository = usedCategoryRepository;
            _logger = logger;
        }

        public async Task Handle(CategoryUsedEvent categoryUsedEvent)
        {
            _logger.LogInformation($"{nameof(CategoryUsedEvent)} with name: {categoryUsedEvent.Name}, has been used in {categoryUsedEvent.UsedIn}");

            var categoryResult = await _categorySearchHandler.GetByName(new GetCategoryByNameQuery(categoryUsedEvent.Name));

            if (!categoryResult.Success)
            {
                _logger.LogInformation($"{nameof(CategoryUsedEvent)} with name: {categoryUsedEvent.Name} cannot be find");
                return;
            }

            await _usedCategoryRepository.CreateOrUpdate(categoryResult.Value.Id, categoryUsedEvent.UsedIn);
            _logger.LogProgress(OperationPhase.Finished, string.Empty, categoryUsedEvent.CorrelationId);
        }
    }
}
