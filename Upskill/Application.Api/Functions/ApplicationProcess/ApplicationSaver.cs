using System.Threading.Tasks;
using Application.Api.Events.Internal;
using Application.Commands.Commands;
using Application.Core.Events.ApplicationAddedEvent;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.EventStore;

namespace Application.Api.Functions.ApplicationProcess
{
    public class ApplicationSaver
    {
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore _eventStore;

        public ApplicationSaver(
            IMapper mapper, 
            IEventPublisher eventPublisher, 
            IEventStore eventStore)
        {
            _mapper = mapper;
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
        }

        [FunctionName(nameof(ApplicationSaver))]
        public async Task Run(
            [DurableClient] IDurableOrchestrationClient client,
            [ActivityTrigger] IDurableActivityContext context,
            ILogger log)
        {
            var command = context.GetInput<SaveApplicationCommand>();

            var applicationAddedEvent = _mapper.Map<SaveApplicationCommand, ApplicationAddedEvent>(command);

            var result = await _eventStore.AppendEvent(applicationAddedEvent.Id, applicationAddedEvent);
            
            if (!result.Success)
            {
                var applicationSaveFailedEvent = new ApplicationSaveFailedInternalFunctionEvent(result.Errors);
                await client.RaiseEventAsync(context.InstanceId, nameof(ApplicationSaveFailedInternalFunctionEvent), applicationSaveFailedEvent);
            }

            await _eventPublisher.PublishEvent(applicationAddedEvent);

            var applicationSavedEvent = new ApplicationSavedInternalFunctionEvent();
            await client.RaiseEventAsync(context.InstanceId, nameof(ApplicationSavedInternalFunctionEvent), applicationSavedEvent);
        }

        private void LogError(ILogger log, string instanceId, string error)
        {
            log.LogError($"Saving application failed instanceId: {instanceId}, error: {error}");
        }
    }
}