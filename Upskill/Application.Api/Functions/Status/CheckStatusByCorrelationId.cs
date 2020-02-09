using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Upskill.LogChecker.Providers;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Application.Api.Functions.Status
{
    public class CheckStatusByCorrelationId
    {
        private readonly ILastLogProvider _lastLogProvider;

        public CheckStatusByCorrelationId(ILastLogProvider lastLogProvider)
        {
            _lastLogProvider = lastLogProvider;
        }

        [FunctionName(nameof(CheckStatusByCorrelationId))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Get, Route = "check/{correlationId}")] HttpRequest req,
            string correlationId)
        {
            var statusResult = await _lastLogProvider.GetLastLogByCorrelationId(correlationId);

            if (!statusResult.Success)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(statusResult.Value);
        }
    }
}
