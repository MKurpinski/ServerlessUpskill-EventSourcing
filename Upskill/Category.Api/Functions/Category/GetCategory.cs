using System.Threading.Tasks;
using Category.Search.Handlers;
using Category.Search.Queries;
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
        private readonly ICategorySearchHandler _categorySearchHandler;

        public GetCategory(ICategorySearchHandler categorySearchHandler)
        {
            _categorySearchHandler = categorySearchHandler;
        }


        [FunctionName(nameof(GetCategory))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Get, Route = "category/{id:guid}")] HttpRequest req,
            string id)
        {
            var categoryResult = await _categorySearchHandler.GetById(new GetCategoryByIdQuery(id));

            if (!categoryResult.Success)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(categoryResult.Value);
        }
    }
}
