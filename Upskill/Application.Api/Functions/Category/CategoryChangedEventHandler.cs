using System.Threading.Tasks;
using Application.Api.Events.External.Category;
using Application.Storage.Dtos;
using Application.Storage.Tables.Repositories;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Api.Functions.Category
{
    public class CategoryChangedEventHandler
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryChangedEventHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [FunctionName(nameof(CategoryChangedEventHandler))]
        public async Task Run([EventGridTrigger]EventGridEvent eventGridEvent,
            ILogger log)
        {
            var categoryChangedEvent = JsonConvert.DeserializeObject<CategoryChangedEvent>(eventGridEvent.Data.ToString());

            var existingCategoryResult = await _categoryRepository.GetById(categoryChangedEvent.Id);

            if (existingCategoryResult.Success && !existingCategoryResult.Value.Name.Equals(categoryChangedEvent.Name))
            {
                // name changed, update the applications
            }

            await _categoryRepository.CreateOrUpdate(new CategoryDto(categoryChangedEvent.Id, categoryChangedEvent.Name));

            log.LogInformation($"{nameof(CategoryChangedEventHandler)}: category: {categoryChangedEvent.Id}");
        }
    }
}
