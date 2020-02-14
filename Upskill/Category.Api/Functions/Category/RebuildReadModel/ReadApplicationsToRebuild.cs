using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventStore.Providers;
using Upskill.Logging.TelemetryInitialization;

namespace Category.Api.Functions.Category.RebuildReadModel
{
    public class ReadCategoriesToRebuild
    {
        private readonly IStreamLogProvider<Core.Aggregates.Category> _streamLogProvider;
        private readonly ITelemetryInitializer _telemetryInitializer;

        public ReadCategoriesToRebuild(
            IStreamLogProvider<Core.Aggregates.Category> streamLogProvider,
            ITelemetryInitializer telemetryInitializer)
        {
            _streamLogProvider = streamLogProvider;
            _telemetryInitializer = telemetryInitializer;
        }

        [FunctionName(nameof(ReadCategoriesToRebuild))]
        public async Task<IReadOnlyCollection<string>> Run(
            [ActivityTrigger] IDurableActivityContext context)
        {
            _telemetryInitializer.Initialize(context.InstanceId);
            var application = await _streamLogProvider.GetStreams();
            return application;
        }
    }
}