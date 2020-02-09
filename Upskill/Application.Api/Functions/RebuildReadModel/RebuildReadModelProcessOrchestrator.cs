using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Upskill.Infrastructure.Enums;
using Upskill.Infrastructure.Extensions;

namespace Application.Api.Functions.RebuildReadModel
{
    public class RebuildReadModelProcessOrchestrator
    {
        [FunctionName(nameof(RebuildReadModelProcessOrchestrator))]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            await context.CallActivityAsync(nameof(StartReindex), null);

            var applicationIds =
                await context.CallActivityAsync<IReadOnlyCollection<string>>(nameof(ReadApplicationsToRebuild), null);

            var rebuildTasks =
                applicationIds.Select(id => context.CreateEntityProxy<IApplicationEntity>(id).Reindex());

            await Task.WhenAll(rebuildTasks);
            log.LogProgress(OperationPhase.InProgress, "Rebuild of events stored in event store finished", context.InstanceId);


            applicationIds =
                await context.CallActivityAsync<IReadOnlyCollection<string>>(nameof(ReadApplicationsToRebuild), null);

            var applyEventTasks =
                applicationIds.Select(id => context.CreateEntityProxy<IApplicationEntity>(id).ApplyPendingEvents());

            await Task.WhenAll(applyEventTasks);
            log.LogProgress(OperationPhase.InProgress, "Applied pending events", context.InstanceId);

            var deleteStateTasks =
                applicationIds.Select(id => context.CreateEntityProxy<IApplicationEntity>(id).Delete());

            await Task.WhenAll(deleteStateTasks);

            await context.CallActivityAsync(nameof(FinishReindex), null);
            log.LogProgress(OperationPhase.Finished, string.Empty, context.InstanceId);
        }
    }
}