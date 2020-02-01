using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Category.Api.Functions.Category.Rebuild
{
    public static class RebuildReadModelProcessOrchestrator
    {
        [FunctionName(nameof(RebuildReadModelProcessOrchestrator))]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            [DurableClient] IDurableOrchestrationClient processStarter)
        {
            //await context.CallActivityAsync(nameof(StartReindex), null);

            //await context.CallActivityAsync(nameof(FinishReindex), null);
        }
    }
}