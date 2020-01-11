using System.Threading.Tasks;
using Category.Storage.Tables.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Category.Api.Functions.Category
{
    public class GetCategory
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategory(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        [FunctionName(nameof(GetCategory))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Get, Route = "category/{id:guid}")] HttpRequest req,
            string id)
        {
            var categoryResult = await _categoryRepository.GetById(id);

            if (!categoryResult.Success)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(categoryResult.Value);
        }
    }
}
