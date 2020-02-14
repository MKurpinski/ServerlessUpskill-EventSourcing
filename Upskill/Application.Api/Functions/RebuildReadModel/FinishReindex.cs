using System.Threading.Tasks;
using Application.Core.Events;
using Application.Search.Managers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.Logging.TelemetryInitialization;

namespace Application.Api.Functions.RebuildReadModel
{
    public class FinishReindex
    {
        private readonly ISearchableApplicationReindexManager _reindexManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly ITelemetryInitializer _telemetryInitializer;

        public FinishReindex(
            ISearchableApplicationReindexManager reindexManager,
            IEventPublisher eventPublisher, 
            ITelemetryInitializer telemetryInitializer)
        {
            _reindexManager = reindexManager;
            _eventPublisher = eventPublisher;
            _telemetryInitializer = telemetryInitializer;
        }

        [FunctionName(nameof(FinishReindex))]
        public async Task Run(
            [ActivityTrigger] IDurableActivityContext context)
        {
            _telemetryInitializer.Initialize(context.InstanceId);
            await _reindexManager.FinishReindexing();
            await _eventPublisher.PublishEvent(new ApplicationReindexFinishedEvent(context.InstanceId));
        }
    }
}