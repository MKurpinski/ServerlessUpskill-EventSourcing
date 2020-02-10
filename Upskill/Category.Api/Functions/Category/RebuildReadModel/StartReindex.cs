using System.Threading.Tasks;
using Category.Core.Events;
using Category.Search.Managers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.FunctionUtils.Extensions;

namespace Category.Api.Functions.Category.RebuildReadModel
{
    public class StartReindex
    {
        private readonly ISearchableCategoryReindexManager _reindexManager;
        private readonly IEventPublisher _eventPublisher;

        public StartReindex(
            ISearchableCategoryReindexManager reindexManager,
            IEventPublisher eventPublisher)
        {
            _reindexManager = reindexManager;
            _eventPublisher = eventPublisher;
        }

        [FunctionName(nameof(StartReindex))]
        public async Task Run(
            [ActivityTrigger] IDurableActivityContext context,
            ExecutionContext executionContext)
        {
            executionContext.CorrelateExecution(context.InstanceId);
            await _reindexManager.StartReindex();
            await _eventPublisher.PublishEvent(new CategoriesReindexStartedEvent(context.InstanceId));
        }
    }
}