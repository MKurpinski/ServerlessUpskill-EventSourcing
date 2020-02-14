using System.Threading.Tasks;
using Application.Api.Events.Internal;
using Application.Commands.Commands;
using Application.Core.Events.CreateApplicationProcessStarted;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.EventStore;
using Upskill.Infrastructure.Enums;
using Upskill.Infrastructure.Extensions;
using Upskill.Logging.TelemetryInitialization;

namespace Application.Api.Functions.ApplicationProcess
{
    public class ApplicationSaver
    {
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore<Core.Aggregates.Application> _eventStore;
        private readonly ITelemetryInitializer _telemetryInitializer;

        public ApplicationSaver(
            IMapper mapper, 
            IEventPublisher eventPublisher,
            IEventStore<Core.Aggregates.Application> eventStore,
            ITelemetryInitializer telemetryInitializer)
        {
            _mapper = mapper;
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
            _telemetryInitializer = telemetryInitializer;
        }

        [FunctionName(nameof(ApplicationSaver))]
        public async Task Run(
            [DurableClient] IDurableOrchestrationClient client,
            [ActivityTrigger] IDurableActivityContext context,
            ILogger log)
        {
            _telemetryInitializer.Initialize(context.InstanceId);
            var command = context.GetInput<CreateApplicationCommand>();

            var applicationCreatedEvent = _mapper.Map<CreateApplicationCommand, CreateApplicationProcessStartedEvent>(command);

            var result = await _eventStore.AppendEvent(applicationCreatedEvent.Id, applicationCreatedEvent);
            
            if (!result.Success)
            {
                var applicationSaveFailedEvent = new ApplicationSaveFailedInternalFunctionEvent(result.Errors);
                await client.RaiseEventAsync(context.InstanceId, nameof(ApplicationSaveFailedInternalFunctionEvent), applicationSaveFailedEvent);
            }

            log.LogProgress(OperationPhase.InProgress, "Application accepted", context.InstanceId);
            await _eventPublisher.PublishEvent(applicationCreatedEvent);

            var applicationSavedEvent = new ApplicationSavedInternalFunctionEvent();
            await client.RaiseEventAsync(context.InstanceId, nameof(ApplicationSavedInternalFunctionEvent), applicationSavedEvent);
        }
    }
}