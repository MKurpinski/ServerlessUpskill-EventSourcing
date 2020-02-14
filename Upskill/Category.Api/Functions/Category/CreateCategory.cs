using System.Threading.Tasks;
using Category.Api.CustomHttpRequests;
using Category.Core.Events;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.EventStore;
using Upskill.FunctionUtils.Results;
using Upskill.Infrastructure;
using Upskill.Infrastructure.Enums;
using Upskill.Infrastructure.Extensions;
using Upskill.Logging.TelemetryInitialization;
using Upskill.RealTimeNotifications.NotificationSubscriberBinding;
using Upskill.RealTimeNotifications.Subscribers;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Category.Api.Functions.Category
{
    public class CreateCategory
    {
        private readonly IGuidProvider _guidProvider;
        private readonly IValidator<CreateCategoryHttpRequest> _createCategoryRequestValidator;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore<Core.Aggregates.Category> _eventStore;
        private readonly ISubscriber _subscriber;
        private readonly ITelemetryInitializer _telemetryInitializer;

        public CreateCategory(
            IValidator<CreateCategoryHttpRequest> createCategoryRequestValidator,
            IGuidProvider guidProvider,
            IEventPublisher eventPublisher,
            IEventStore<Core.Aggregates.Category> eventStore, 
            ISubscriber subscriber,
            ITelemetryInitializer telemetryInitializer)
        {
            _createCategoryRequestValidator = createCategoryRequestValidator;
            _guidProvider = guidProvider;
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
            _subscriber = subscriber;
            _telemetryInitializer = telemetryInitializer;
        }


        [FunctionName(nameof(CreateCategory))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Post, Route = "category")] CreateCategoryHttpRequest createCategoryRequest,
            [NotificationSubscriber] string subscriber,
            ILogger log)
        {
            var id = _guidProvider.GenerateGuid();
            _telemetryInitializer.Initialize(id);
            var validationResult = await _createCategoryRequestValidator.ValidateAsync(createCategoryRequest);
            var correlationId = id;

            log.LogProgress(OperationPhase.Started, "Creating category process started", correlationId);

            if (!validationResult.IsValid)
            {
                log.LogProgress(OperationPhase.Failed, "Validation failed", correlationId);
                return new BadRequestObjectResult(validationResult.Errors);
            }

            await _subscriber.Register(correlationId, subscriber);

            var categoryAddedEvent = new CreateCategoryProcessStartedEvent(
                id,
                createCategoryRequest.Name,
                createCategoryRequest.Description,
                createCategoryRequest.SortOrder,
                correlationId);

            var saveEventResult = await _eventStore.AppendEvent(id, categoryAddedEvent);

            if (!saveEventResult.Success)
            {
                log.LogFailedOperation(OperationPhase.Failed, "Creating category process failed", saveEventResult.Errors, correlationId);
                return new BadRequestResult();
            }

            log.LogProgress(OperationPhase.InProgress, "Request accepted to further processing.", correlationId);
            await _eventPublisher.PublishEvent(categoryAddedEvent);

            return new AcceptedWithCorrelationIdHeaderResult(correlationId);
        }
    }
}
