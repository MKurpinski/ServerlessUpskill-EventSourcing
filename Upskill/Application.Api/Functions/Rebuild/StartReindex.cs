using System.Threading.Tasks;
using Application.Core.Events;
using Application.Search.Managers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventsInfrastructure.Publishers;

namespace Application.Api.Functions.Rebuild
{
    public class StartReindex
    {
        private readonly ISearchableApplicationReindexManager _reindexManager;
        private readonly IEventPublisher _eventPublisher;

        public StartReindex(
            ISearchableApplicationReindexManager reindexManager,
            IEventPublisher eventPublisher)
        {
            _reindexManager = reindexManager;
            _eventPublisher = eventPublisher;
        }

        [FunctionName(nameof(StartReindex))]
        public async Task Run([ActivityTrigger] IDurableActivityContext context)
        {
            await _reindexManager.StartReindex();
            await _eventPublisher.PublishEvent(new ApplicationReindexStartedEvent(context.InstanceId));
        }
    }
}