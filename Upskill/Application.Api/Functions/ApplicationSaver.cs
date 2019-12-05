using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Application.Api.Functions
{
    public class ApplicationSaver
    {
        [FunctionName(nameof(ApplicationSaver))]
        public async Task Run(
            [DurableClient] IDurableOrchestrationClient client, 
            ILogger log)
        {
            log.LogInformation($"Saying hello to.");
            //client.RaiseEventAsync();
        }
    }
}