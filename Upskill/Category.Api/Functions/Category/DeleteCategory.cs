using System.Threading.Tasks;
using Category.Core.Events;
using Category.DataStorage.Repositories;
using Category.Storage.Tables.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Upskill.EventsInfrastructure.Publishers;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Category.Api.Functions.Category
{
    public class DeleteCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUsedCategoryRepository _usedCategoryRepository;
        private readonly IEventPublisher _eventPublisher;

        public DeleteCategory(
            ICategoryRepository categoryRepository,
            IUsedCategoryRepository usedCategoryRepository,
            IEventPublisher eventPublisher)
        {
            _categoryRepository = categoryRepository;
            _usedCategoryRepository = usedCategoryRepository;
            _eventPublisher = eventPublisher;
        }

        [FunctionName(nameof(DeleteCategory))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Delete, Route = "category/{id:guid}")] HttpRequest req,
            string id)
        {
            var category = await _categoryRepository.GetById(id);

            if (!category.Success)
            {
                return new NotFoundResult();
            }

            var categoryUsage = await _usedCategoryRepository.GetCategoryUsageById(id);

            var canDelete = categoryUsage.UsageCounter == default;

            if (!canDelete)
            {
                return new BadRequestObjectResult("Cannot delete the category, when it's used");
            }

            var deleteResult = await _categoryRepository.Delete(id);

            if (!deleteResult.Success)
            {
                return new NotFoundResult();
            }

            await _eventPublisher.PublishEvent(new CategoryDeletedEvent(id));
            return new NoContentResult();
        }
    }
}
