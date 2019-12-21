using System.Threading.Tasks;
using Category.DataStorage.Repositories;
using Category.Storage.Tables.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Category.Api.Functions
{
    public class DeleteCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUsedCategoryRepository _usedCategoryRepository;

        public DeleteCategory(
            ICategoryRepository categoryRepository,
            IUsedCategoryRepository usedCategoryRepository)
        {
            _categoryRepository = categoryRepository;
            _usedCategoryRepository = usedCategoryRepository;
        }

        [FunctionName(nameof(DeleteCategory))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Delete, Route = "category/{id:guid}")] HttpRequest req,
            string id)
        {
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

            return new NoContentResult();
        }
    }
}
