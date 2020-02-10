using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Upskill.FunctionUtils.Extensions;
using Upskill.Infrastructure.Enums;
using Upskill.Infrastructure.Extensions;

namespace Category.Api.Functions.Category.RebuildReadModel
{
    public class RebuildReadModelProcessOrchestrator
    {
        [FunctionName(nameof(RebuildReadModelProcessOrchestrator))]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context, 
            ILogger log,
            ExecutionContext executionContext)
        {
            executionContext.CorrelateExecution(context.InstanceId);
            await context.CallActivityAsync(nameof(StartReindex), null);

            var categoryIds =
                await context.CallActivityAsync<IReadOnlyCollection<string>>(nameof(ReadCategoriesToRebuild), null);

            var rebuildTasks = 
                categoryIds.Select(id => context.CreateEntityProxy<ICategoryEntity>(id).Reindex());

            await Task.WhenAll(rebuildTasks);
            log.LogProgress(OperationPhase.InProgress, "Rebuild of events stored in event store finished", context.InstanceId);

            categoryIds =
                await context.CallActivityAsync<IReadOnlyCollection<string>>(nameof(ReadCategoriesToRebuild), null);


            var applyEventTasks =
                categoryIds.Select(id => context.CreateEntityProxy<ICategoryEntity>(id).ApplyPendingEvents());

            await Task.WhenAll(applyEventTasks);
            log.LogProgress(OperationPhase.InProgress, "Applied pending events", context.InstanceId);


            var deleteStateTasks =
                categoryIds.Select(id => context.CreateEntityProxy<ICategoryEntity>(id).Delete());

            await Task.WhenAll(deleteStateTasks);

            await context.CallActivityAsync(nameof(FinishReindex), null);
            log.LogProgress(OperationPhase.Finished, string.Empty, context.InstanceId);
        }
    }
}