using System.Threading.Tasks;
using Category.Core.Events;
using Category.Search.Managers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventsInfrastructure.Publishers;

namespace Category.Api.Functions.Category.Rebuild
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
        public async Task Run([ActivityTrigger] IDurableActivityContext context)
        {
            await _reindexManager.FinishReindexing();
            await _eventPublisher.PublishEvent(new CategoriesReindexFinishedEvent(context.InstanceId));
        }
    }
}