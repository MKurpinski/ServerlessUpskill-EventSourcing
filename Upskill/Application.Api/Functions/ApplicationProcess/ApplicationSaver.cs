using System;
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
using Upskill.Telemetry.CorrelationInitializers;

namespace Application.Api.Functions.ApplicationProcess
{
    public class ApplicationSaver
    {
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore<Core.Aggregates.Application> _eventStore;
        private readonly ICorrelationInitializer _correlationInitializer;

        public ApplicationSaver(
            IMapper mapper, 
            IEventPublisher eventPublisher,
            IEventStore<Core.Aggregates.Application> eventStore,
            ICorrelationInitializer correlationInitializer)
        {
            _mapper = mapper;
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
            _correlationInitializer = correlationInitializer;
        }

        [FunctionName(nameof(ApplicationSaver))]
        public async Task Run(
            [DurableClient] IDurableOrchestrationClient client,
            [ActivityTrigger] IDurableActivityContext context,
            ILogger log)
        {
            _correlationInitializer.Initialize(context.InstanceId);
            var command = context.GetInput<CreateApplicationCommand>();

            var applicationCreatedEvent = _mapper.Map<CreateApplicationCommand, CreateApplicationProcessStartedEvent>(command);

            var result = await _eventStore.AppendEvent(applicationCreatedEvent.Id, applicationCreatedEvent);
            
            if (!result.Success)
            {
                var applicationSaveFailedEvent = new ApplicationSaveFailedInternalFunctionEvent(result.Errors);
                await client.RaiseEventAsync(context.InstanceId, nameof(ApplicationSaveFailedInternalFunctionEvent), applicationSaveFailedEvent);
            }

            log.LogProgress(OperationStatus.InProgress, "Application accepted", context.InstanceId);
            await _eventPublisher.PublishEvent(applicationCreatedEvent);

            var applicationSavedEvent = new ApplicationSavedInternalFunctionEvent();
            await client.RaiseEventAsync(context.InstanceId, nameof(ApplicationSavedInternalFunctionEvent), applicationSavedEvent);
        }
    }
}