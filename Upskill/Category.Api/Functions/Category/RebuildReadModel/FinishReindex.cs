using System.Threading.Tasks;
using Category.Core.Events;
using Category.Search.Managers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.Telemetry.CorrelationInitializers;

namespace Category.Api.Functions.Category.RebuildReadModel
{
    public class FinishReindex
    {
        private readonly ISearchableCategoryReindexManager _reindexManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICorrelationInitializer _correlationInitializer;

        public FinishReindex(
            ISearchableCategoryReindexManager reindexManager,
            IEventPublisher eventPublisher,
            ICorrelationInitializer correlationInitializer)
        {
            _reindexManager = reindexManager;
            _eventPublisher = eventPublisher;
            _correlationInitializer = correlationInitializer;
        }

        [FunctionName(nameof(FinishReindex))]
        public async Task Run(
            [ActivityTrigger] IDurableActivityContext context)
        {
            _correlationInitializer.Initialize(context.InstanceId);
            await _reindexManager.FinishReindexing();
            await _eventPublisher.PublishEvent(new CategoriesReindexFinishedEvent(context.InstanceId));
        }
    }
}