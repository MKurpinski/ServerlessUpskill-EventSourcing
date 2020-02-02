using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Application.Api.Functions.RebuildReadModel
{
    public class RebuildReadModelProcessOrchestrator
    {
        [FunctionName(nameof(RebuildReadModelProcessOrchestrator))]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            [DurableClient] IDurableEntityClient client)
        {
            await context.CallActivityAsync(nameof(StartReindex), null);

            var applicationIds =
                await context.CallActivityAsync<IReadOnlyCollection<string>>(nameof(ReadApplicationsToRebuild), null);

            var rebuildTasks =
                applicationIds.Select(id => client.SignalEntityAsync<IApplicationEntity>(id, proxy => proxy.Reindex()));

            await Task.WhenAll(rebuildTasks);

            applicationIds =
                await context.CallActivityAsync<IReadOnlyCollection<string>>(nameof(ReadApplicationsToRebuild), null);

            var applyEventTasks = 
                applicationIds.Select(id => client.SignalEntityAsync<IApplicationEntity>(id, proxy => proxy.ApplyPendingEvents()));

            await Task.WhenAll(applyEventTasks);

            var deleteStateTasks = 
                applicationIds.Select(id => client.SignalEntityAsync<IApplicationEntity>(id, proxy => proxy.Delete()));

            await Task.WhenAll(deleteStateTasks);

            await context.CallActivityAsync(nameof(FinishReindex), null);
        }
    }
}