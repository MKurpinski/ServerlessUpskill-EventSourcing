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
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            await context.CallActivityAsync(nameof(StartReindex), null);

            var applicationIds =
                await context.CallActivityAsync<IReadOnlyCollection<string>>(nameof(ReadApplicationsToRebuild), null);

            var rebuildTasks = applicationIds.Select(id =>
            {
                var proxy = this.GetApplicationEntityProxy(context, id);
                return proxy.Reindex();
            });

            await Task.WhenAll(rebuildTasks);

            applicationIds =
                await context.CallActivityAsync<IReadOnlyCollection<string>>(nameof(ReadApplicationsToRebuild), null);

            var applyEventTasks = applicationIds.Select(id =>
            {
                var proxy = this.GetApplicationEntityProxy(context, id);
                return proxy.ApplyPendingEvents();
            });

            await Task.WhenAll(applyEventTasks);

            var deleteStateTasks = applicationIds.Select(id =>
            {
                var proxy = this.GetApplicationEntityProxy(context, id);
                return proxy.Delete();
            });

            await Task.WhenAll(deleteStateTasks);

            await context.CallActivityAsync(nameof(FinishReindex), null);
        }

        private IApplicationEntity GetApplicationEntityProxy(IDurableOrchestrationContext context, string id)
        {
            return context.CreateEntityProxy<IApplicationEntity>(id);
        }
    }
}