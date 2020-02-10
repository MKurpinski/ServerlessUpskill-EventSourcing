using System.Threading.Tasks;
using Application.Core.Events;
using Application.Search.Managers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.FunctionUtils.Extensions;

namespace Application.Api.Functions.RebuildReadModel
{
    public class FinishReindex
    {
        private readonly ISearchableApplicationReindexManager _reindexManager;
        private readonly IEventPublisher _eventPublisher;

        public FinishReindex(
            ISearchableApplicationReindexManager reindexManager,
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
            await _eventPublisher.PublishEvent(new ApplicationReindexFinishedEvent(context.InstanceId));
        }
    }
}