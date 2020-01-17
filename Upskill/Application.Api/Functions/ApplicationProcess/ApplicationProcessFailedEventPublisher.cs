using System.Threading.Tasks;
using Application.Core.Events;
using Application.EventStore.Facades;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventsInfrastructure.Publishers;

namespace Application.Api.Functions.ApplicationProcess
{
    public class ApplicationProcessFailedEventPublisher
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStoreFacade _eventStore;

        public ApplicationProcessFailedEventPublisher(
            IEventPublisher eventPublisher,
            IEventStoreFacade eventStore)
        {
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
        }

        [FunctionName(nameof(ApplicationProcessFailedEventPublisher))]
        public async Task Run(
            [ActivityTrigger] IDurableActivityContext context)
        {
            var correlationId = context.GetInput<string>();
            var applicationFailedEvent = new ApplicationAddingFailedEvent(correlationId);
            await _eventStore.AppendEvent(correlationId, applicationFailedEvent);
            await _eventPublisher.PublishEvent(applicationFailedEvent);
        }
    }
}