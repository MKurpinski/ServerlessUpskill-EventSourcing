using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventStore.Providers;
using Upskill.FunctionUtils.Extensions;

namespace Application.Api.Functions.RebuildReadModel
{
    public class ReadApplicationsToRebuild
    {
        private readonly IStreamLogProvider<Core.Aggregates.Application> _streamLogProvider;

        public ReadApplicationsToRebuild(
            IStreamLogProvider<Core.Aggregates.Application> streamLogProvider)
        {
            _streamLogProvider = streamLogProvider;
        }

        [FunctionName(nameof(ReadApplicationsToRebuild))]
        public async Task<IReadOnlyCollection<string>> Run(
            [ActivityTrigger] IDurableActivityContext context,
            ExecutionContext executionContext)
        {
            executionContext.CorrelateExecution(context.InstanceId);
            var application = await _streamLogProvider.GetStreams();
            return application;
        }
    }
}