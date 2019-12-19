using System.Threading.Tasks;
using Application.Api.CustomHttpRequests;
using Application.Search.Handlers;
using Application.Search.Queries;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using HttpMethods = Application.Api.Constants.HttpMethods;

namespace Application.Api.Functions.Search
{
    public class SimpleSearchApplication
    {
        private readonly IApplicationSearchHandler _applicationSearchHandler;
        private readonly IValidator<SimpleApplicationSearchHttpRequest> _requestValidator;

        public SimpleSearchApplication(
            IApplicationSearchHandler applicationSearchHandler,
            IValidator<SimpleApplicationSearchHttpRequest> requestValidator)
        {
            _applicationSearchHandler = applicationSearchHandler;
            _requestValidator = requestValidator;
        }

        [FunctionName(nameof(SimpleSearchApplication))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Post, Route = "search")] SimpleApplicationSearchHttpRequest req)
        {
            var requestValidationResult = await _requestValidator.ValidateAsync(req);
            if (!requestValidationResult.IsValid)
            {
                return new BadRequestObjectResult(requestValidationResult.Errors);
            }

            var results =
                await _applicationSearchHandler.Search(new SimpleApplicationSearchQuery(req.Query, req.Skip, req.Take));
            return new OkObjectResult(results);
        }
    }
}
