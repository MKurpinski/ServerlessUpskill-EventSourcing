using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Category.Api.Functions.Category.RebuildReadModel
{
    public class RebuildReadModelProcessOrchestrator
    {
        [FunctionName(nameof(RebuildReadModelProcessOrchestrator))]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            await context.CallActivityAsync(nameof(StartReindex), null);

            var categoryIds =
                await context.CallActivityAsync<IReadOnlyCollection<string>>(nameof(ReadCategoriesToRebuild), null);

            var rebuildTasks = 
                categoryIds.Select(id => context.CreateEntityProxy<ICategoryEntity>(id).Reindex());

            await Task.WhenAll(rebuildTasks);

            categoryIds =
                await context.CallActivityAsync<IReadOnlyCollection<string>>(nameof(ReadCategoriesToRebuild), null);

            var applyEventTasks =
                categoryIds.Select(id => context.CreateEntityProxy<ICategoryEntity>(id).ApplyPendingEvents());

            await Task.WhenAll(applyEventTasks);

            var deleteStateTasks =
                categoryIds.Select(id => context.CreateEntityProxy<ICategoryEntity>(id).Delete());

            await Task.WhenAll(deleteStateTasks);

            await context.CallActivityAsync(nameof(FinishReindex), null);
        }
    }
}