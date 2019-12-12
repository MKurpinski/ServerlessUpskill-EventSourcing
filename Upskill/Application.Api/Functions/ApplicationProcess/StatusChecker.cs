using System.Threading.Tasks;
using Application.ProcessStatus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using HttpMethods = Application.Api.Constants.HttpMethods;

namespace Application.Api.Functions.ApplicationProcess
{
    public class StatusChecker
    {
        private readonly IProcessStatusHandler _processStatusHandler;

        public StatusChecker(IProcessStatusHandler processStatusHandler)
        {
            _processStatusHandler = processStatusHandler;
        }

        [FunctionName(nameof(StatusChecker))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Get, Route = "status/{correlationId}")] HttpRequest req,
            string correlationId)
        {
            var statusResult = await _processStatusHandler.GetStatus(correlationId);

            if (!statusResult.Success)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(statusResult.Value);
        }
    }
}
