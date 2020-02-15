using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventStore.Providers;
using Upskill.Telemetry.CorrelationInitializers;

namespace Application.Api.Functions.RebuildReadModel
{
    public class ReadApplicationsToRebuild
    {
        private readonly IStreamLogProvider<Core.Aggregates.Application> _streamLogProvider;
        private readonly ICorrelationInitializer _correlationInitializer;

        public ReadApplicationsToRebuild(
            IStreamLogProvider<Core.Aggregates.Application> streamLogProvider,
            ICorrelationInitializer correlationInitializer)
        {
            _streamLogProvider = streamLogProvider;
            _correlationInitializer = correlationInitializer;
        }

        [FunctionName(nameof(ReadApplicationsToRebuild))]
        public async Task<IReadOnlyCollection<string>> Run(
            [ActivityTrigger] IDurableActivityContext context)
        {
            _correlationInitializer.Initialize(context.InstanceId);
            var application = await _streamLogProvider.GetStreams();
            return application;
        }
    }
}