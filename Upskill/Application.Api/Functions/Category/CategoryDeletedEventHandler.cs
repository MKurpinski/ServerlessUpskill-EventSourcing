using System.Threading.Tasks;
using Application.Api.Events.External.Category;
using Application.Storage.Tables.Repositories;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Newtonsoft.Json;

namespace Application.Api.Functions.Category
{
    public class CategoryDeletedEventHandler
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryDeletedEventHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [FunctionName(nameof(CategoryDeletedEventHandler))]
        public async Task Run([EventGridTrigger]EventGridEvent eventGridEvent)
        {
            var categoryDeletedEvent = JsonConvert.DeserializeObject<CategoryDeletedEvent>(eventGridEvent.Data.ToString());
            await _categoryRepository.Delete(categoryDeletedEvent.Id);
        }
    }
}
