using System.Threading.Tasks;
using Category.Core.Events;
using Category.Search.Managers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.FunctionUtils.Extensions;

namespace Category.Api.Functions.Category.RebuildReadModel
{
    public class FinishReindex
    {
        private readonly ISearchableCategoryReindexManager _reindexManager;
        private readonly IEventPublisher _eventPublisher;

        public FinishReindex(
            ISearchableCategoryReindexManager reindexManager,
            IEventPublisher eventPublisher)
        {
            _reindexManager = reindexManager;
            _eventPublisher = eventPublisher;
        }

        [FunctionName(nameof(FinishReindex))]
        public async Task Run(
            [ActivityTrigger] IDurableActivityContext context,
            ExecutionContext executionContext)
        {
            executionContext.CorrelateExecution(context.InstanceId);
            await _reindexManager.FinishReindexing();
            await _eventPublisher.PublishEvent(new CategoriesReindexFinishedEvent(context.InstanceId));
        }
    }
}