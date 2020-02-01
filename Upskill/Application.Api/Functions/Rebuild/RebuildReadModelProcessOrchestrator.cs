using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Application.Api.Functions.Rebuild
{
    public class RebuildReadModelProcessOrchestrator
    {
        [FunctionName(nameof(RebuildReadModelProcessOrchestrator))]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            [DurableClient] IDurableOrchestrationClient processStarter)
        {
            await context.CallActivityAsync(nameof(StartReindex), null);

            var applicationIds =
                await context.CallActivityAsync<IReadOnlyCollection<string>>(nameof(ReadApplicationsToRebuild), null);

            var rebuildTasks = applicationIds.Select(id =>
            {
                var proxy = this.GetApplicationEntityProxy(context, id);
                return proxy.Rebuild();
            });

            await Task.WhenAll(rebuildTasks);

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