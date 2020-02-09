using System.Threading.Tasks;
using Category.Api.Commands;
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
using Upskill.RealTimeNotifications.NotificationSubscriberBinding;
using Upskill.RealTimeNotifications.Subscribers;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Category.Api.Functions.Category
{
    public class UpdateCategory
    {
        private readonly IValidator<UpdateCategoryCommand> _updateCommandValidator;
        private readonly IGuidProvider _guidProvider;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore<Core.Aggregates.Category> _eventStore;
        private readonly ISubscriber _subscriber;

        public UpdateCategory(
            IValidator<UpdateCategoryCommand> updateCommandValidator,
            IEventPublisher eventPublisher,
            IEventStore<Core.Aggregates.Category> eventStore, 
            IGuidProvider guidProvider,
            ISubscriber subscriber)
        {
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
            _guidProvider = guidProvider;
            _subscriber = subscriber;
            _updateCommandValidator = updateCommandValidator;
        }

        [FunctionName(nameof(UpdateCategory))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Put, Route = "category/{id:guid}")] UpdateCategoryHttpRequest updateCategoryRequest,
            string id,
            [NotificationSubscriber] string subscriber,
            ILogger log)
        {
            var correlationId = _guidProvider.GenerateGuid(); 
            var validationResult = await _updateCommandValidator.ValidateAsync(new UpdateCategoryCommand(id, updateCategoryRequest));
            log.LogProgress(OperationPhase.Started, "Updating category process started", correlationId);

            if (!validationResult.IsValid)
            {
                log.LogProgress(OperationPhase.Failed, "Validation failed", correlationId);
                return new BadRequestObjectResult(validationResult.Errors);
            }

            await _subscriber.Register(correlationId, subscriber);

            var categoryChangedEvent = new UpdateCategoryProcessStartedEvent(
                id,
                updateCategoryRequest.Name,
                updateCategoryRequest.Description,
                updateCategoryRequest.SortOrder,
                correlationId);

            var saveEventResult = await _eventStore.AppendEvent(id, categoryChangedEvent);

            if (!saveEventResult.Success)
            {
                log.LogFailedOperation(OperationPhase.Failed, "Creating category process failed", saveEventResult.Errors, correlationId);
                return new BadRequestResult();
            }

            log.LogProgress(OperationPhase.InProgress, "Request accepted to further processing.", correlationId);
            await _eventPublisher.PublishEvent(categoryChangedEvent);
            log.LogProgress(OperationPhase.InProgress, $"{nameof(UpdateCategoryProcessStartedEvent)} published", correlationId);

            return new AcceptedWithCorrelationIdHeaderResult(correlationId);
        }
    }
}
