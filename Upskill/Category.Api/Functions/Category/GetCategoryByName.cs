using System.Threading.Tasks;
using Category.Search.Handlers;
using Category.Search.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Category.Api.Functions.Category
{
    public class GetCategoryByName
    {
        private readonly ICategorySearchHandler _categorySearchHandler;

        public GetCategoryByName(ICategorySearchHandler categorySearchHandler)
        {
            _categorySearchHandler = categorySearchHandler;
        }


        [FunctionName(nameof(GetCategoryByName))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Get, Route = "category/name/{name}")] HttpRequest req,
            string name)
        {
            var categoryResult = await _categorySearchHandler.GetByName(new GetCategoryByNameQuery(name));

            if (!categoryResult.Success)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(categoryResult.Value);
        }
    }
}
