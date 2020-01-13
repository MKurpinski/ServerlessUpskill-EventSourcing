using System.Threading.Tasks;
using Category.Storage.Tables.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Category.Api.Functions.Category
{
    public class GetCategoryByName
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByName(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        [FunctionName(nameof(GetCategoryByName))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Get, Route = "category/name/{name}")] HttpRequest req,
            string name)
        {
            var categoryResult = await _categoryRepository.GetByName(name);

            if (!categoryResult.Success)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(categoryResult.Value);
        }
    }
}
