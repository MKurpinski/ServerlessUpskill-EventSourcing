using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Application.Api.Functions
{
    public static class ApplicationProcessOrchestrator
    {
        [FunctionName("ApplicationProcessOrchestrator")]
        public static Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            return Task.CompletedTask;
        }
    }
}