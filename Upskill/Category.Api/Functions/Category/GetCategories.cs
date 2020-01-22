using System.Threading.Tasks;
using Category.Api.CustomHttpRequests;
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
    public class GetCategories
    {
        private readonly ICategorySearchHandler _categorySearchHandler;

        public GetCategories(ICategorySearchHandler categorySearchHandler)
        {
            _categorySearchHandler = categorySearchHandler;
        }

        [FunctionName(nameof(GetCategories))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Post, Route = "categories")] GetCategoriesHttpRequest req)
        {
            var categories = await _categorySearchHandler.Get(new GetCategoriesQuery(req.Skip, req.Take));
            return new OkObjectResult(categories);
        }
    }
}
