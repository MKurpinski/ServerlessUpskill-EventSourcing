using System.Threading.Tasks;
using Application.Core.Events;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.EventStore;
using Upskill.Telemetry.CorrelationInitializers;

namespace Application.Api.Functions.ApplicationProcess
{
    public class ApplicationProcessFailedEventPublisher
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore<Core.Aggregates.Application> _eventStore;
        private readonly ICorrelationInitializer _correlationInitializer;

        public ApplicationProcessFailedEventPublisher(
            IEventPublisher eventPublisher,
            IEventStore<Core.Aggregates.Application> eventStore,
            ICorrelationInitializer correlationInitializer)
        {
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
            _correlationInitializer = correlationInitializer;
        }

        [FunctionName(nameof(ApplicationProcessFailedEventPublisher))]
        public async Task Run(
            [ActivityTrigger] IDurableActivityContext context)
        {
            _correlationInitializer.Initialize(context.InstanceId);
            var status = context.GetInput<string>();
            var applicationFailedEvent = new CreatingApplicationFailedEvent(context.InstanceId, status, context.InstanceId);
            await _eventStore.AppendEvent(context.InstanceId, applicationFailedEvent);
            await _eventPublisher.PublishEvent(applicationFailedEvent);
        }
    }
}