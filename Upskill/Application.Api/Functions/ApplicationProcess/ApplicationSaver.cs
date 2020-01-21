using System.Threading.Tasks;
using Application.Api.Events.Internal;
using Application.Commands.Commands;
using Application.Core.Events.CreateApplicationProcessStarted;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.EventStore;

namespace Application.Api.Functions.ApplicationProcess
{
    public class ApplicationSaver
    {
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore<Core.Aggregates.Application> _eventStore;

        public ApplicationSaver(
            IMapper mapper, 
            IEventPublisher eventPublisher,
            IEventStore<Core.Aggregates.Application> eventStore)
        {
            _mapper = mapper;
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
        }

        [FunctionName(nameof(ApplicationSaver))]
        public async Task Run(
            [DurableClient] IDurableOrchestrationClient client,
            [ActivityTrigger] IDurableActivityContext context)
        {
            var command = context.GetInput<CreateApplicationCommand>();

            var applicationCreatedEvent = _mapper.Map<CreateApplicationCommand, CreateApplicationProcessStartedEvent>(command);

            var result = await _eventStore.AppendEvent(applicationCreatedEvent.Id, applicationCreatedEvent);
            
            if (!result.Success)
            {
                var applicationSaveFailedEvent = new ApplicationSaveFailedInternalFunctionEvent(result.Errors);
                await client.RaiseEventAsync(context.InstanceId, nameof(ApplicationSaveFailedInternalFunctionEvent), applicationSaveFailedEvent);
            }

            await _eventPublisher.PublishEvent(applicationCreatedEvent);

            var applicationSavedEvent = new ApplicationSavedInternalFunctionEvent();
            await client.RaiseEventAsync(context.InstanceId, nameof(ApplicationSavedInternalFunctionEvent), applicationSavedEvent);
        }
    }
}