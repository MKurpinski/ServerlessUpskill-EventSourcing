using System.Threading.Tasks;
using Category.Api.Events;
using Category.DataStorage.Repositories;
using Category.Storage.Tables.Repositories;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Category.Api.Functions
{
    public class CategoryUsedEventHandler
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUsedCategoryRepository _usedCategoryRepository;

        public CategoryUsedEventHandler(
            ICategoryRepository categoryRepository,
            IUsedCategoryRepository usedCategoryRepository)
        {
            _categoryRepository = categoryRepository;
            _usedCategoryRepository = usedCategoryRepository;
        }

        [FunctionName(nameof(CategoryUsedEventHandler))]
        public async Task Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            var categoryUsed = JsonConvert.DeserializeObject<CategoryUsedEvent>(eventGridEvent.Data.ToString());
            log.LogInformation($"{nameof(CategoryUsedEvent)} with name: {categoryUsed.Name}, has been used in {categoryUsed.UsedIn}");

            var categoryResult = await _categoryRepository.GetByName(categoryUsed.Name);

            if (!categoryResult.Success)
            {
                log.LogError($"{nameof(CategoryUsedEvent)} with name: {categoryUsed.Name} cannot be find. Inconsistency of data!");
                return;
            }

            await _usedCategoryRepository.CreateOrUpdate(categoryResult.Value.Id, categoryUsed.UsedIn);
        }
    }
}
