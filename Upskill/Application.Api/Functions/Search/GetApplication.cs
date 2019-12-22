using System.Threading.Tasks;
using Application.Search.Handlers;
using Application.Search.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Application.Api.Functions.Search
{
    public class GetApplication
    {
        private readonly IApplicationSearchHandler _applicationSearchHandler;

        public GetApplication(IApplicationSearchHandler applicationSearchHandler)
        {
            _applicationSearchHandler = applicationSearchHandler;
        }

        [FunctionName(nameof(GetApplication))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Get, Route = "search/{id:guid}")] HttpRequest req,
            string id)
        {
            var result = await _applicationSearchHandler.GetById(new GetApplicationByIdQuery(id));
            
            if (!result.Success)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(result.Value);
        }
    }
}
