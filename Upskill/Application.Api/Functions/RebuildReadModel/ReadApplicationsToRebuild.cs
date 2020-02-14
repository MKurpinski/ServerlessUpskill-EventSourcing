using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventStore.Providers;
using Upskill.Logging.TelemetryInitialization;

namespace Application.Api.Functions.RebuildReadModel
{
    public class ReadApplicationsToRebuild
    {
        private readonly IStreamLogProvider<Core.Aggregates.Application> _streamLogProvider;
        private readonly ITelemetryInitializer _telemetryInitializer;

        public ReadApplicationsToRebuild(
            IStreamLogProvider<Core.Aggregates.Application> streamLogProvider,
            ITelemetryInitializer telemetryInitializer)
        {
            _streamLogProvider = streamLogProvider;
            _telemetryInitializer = telemetryInitializer;
        }

        [FunctionName(nameof(ReadApplicationsToRebuild))]
        public async Task<IReadOnlyCollection<string>> Run(
            [ActivityTrigger] IDurableActivityContext context)
        {
            _telemetryInitializer.Initialize(context.InstanceId);
            var application = await _streamLogProvider.GetStreams();
            return application;
        }
    }
}