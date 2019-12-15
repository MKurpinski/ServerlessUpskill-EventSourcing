using System.Threading.Tasks;
using Application.Api.CustomHttpRequests;
using Application.Search.Handlers;
using Application.Search.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using HttpMethods = Application.Api.Constants.HttpMethods;

namespace Application.Api.Functions.Search
{
    public class SimpleSearchApplication
    {
        private readonly IApplicationSearchHandler _applicationSearchHandler;

        public SimpleSearchApplication(IApplicationSearchHandler applicationSearchHandler)
        {
            _applicationSearchHandler = applicationSearchHandler;
        }

        [FunctionName(nameof(SimpleSearchApplication))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Post, Route = "search")] SimpleApplicationSearchHttpRequest req)
        {
            var results =
                await _applicationSearchHandler.Search(new SimpleApplicationSearchQuery(req.Query, req.Skip, req.Take));
            return new OkObjectResult(results);
        }
    }
}
