using System.Threading.Tasks;
using Category.Core.Events;
using Category.Search.Managers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.FunctionUtils.Extensions;
using Upskill.Logging.TelemetryInitialization;

namespace Category.Api.Functions.Category.RebuildReadModel
{
    public class StartReindex
    {
        private readonly ISearchableCategoryReindexManager _reindexManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly ITelemetryInitializer _telemetryInitializer;

        public StartReindex(
            ISearchableCategoryReindexManager reindexManager,
            IEventPublisher eventPublisher,
            ITelemetryInitializer telemetryInitializer)
        {
            _reindexManager = reindexManager;
            _eventPublisher = eventPublisher;
            _telemetryInitializer = telemetryInitializer;
        }

        [FunctionName(nameof(StartReindex))]
        public async Task Run(
            [ActivityTrigger] IDurableActivityContext context)
        {
            _telemetryInitializer.Initialize(context.InstanceId);
            await _reindexManager.StartReindex();
            await _eventPublisher.PublishEvent(new CategoriesReindexStartedEvent(context.InstanceId));
        }
    }
}