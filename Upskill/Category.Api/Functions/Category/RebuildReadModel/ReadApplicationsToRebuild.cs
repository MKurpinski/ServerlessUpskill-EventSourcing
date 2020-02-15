using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventStore.Providers;
using Upskill.Telemetry.CorrelationInitializers;

namespace Category.Api.Functions.Category.RebuildReadModel
{
    public class ReadCategoriesToRebuild
    {
        private readonly IStreamLogProvider<Core.Aggregates.Category> _streamLogProvider;
        private readonly ICorrelationInitializer _correlationInitializer;

        public ReadCategoriesToRebuild(
            IStreamLogProvider<Core.Aggregates.Category> streamLogProvider,
            ICorrelationInitializer correlationInitializer)
        {
            _streamLogProvider = streamLogProvider;
            _correlationInitializer = correlationInitializer;
        }

        [FunctionName(nameof(ReadCategoriesToRebuild))]
        public async Task<IReadOnlyCollection<string>> Run(
            [ActivityTrigger] IDurableActivityContext context)
        {
            _correlationInitializer.Initialize(context.InstanceId);
            var application = await _streamLogProvider.GetStreams();
            return application;
        }
    }
}