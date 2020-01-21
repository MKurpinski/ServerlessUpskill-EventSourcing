using System.Threading.Tasks;
using Category.Api.CustomHttpRequests;
using Category.Core.Events.Internal;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.EventStore;
using Upskill.FunctionUtils.Results;
using Upskill.Infrastructure;
using Upskill.Infrastructure.Extensions;
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

        public CreateCategory(
            IValidator<CreateCategoryHttpRequest> createCategoryRequestValidator,
            IGuidProvider guidProvider,
            IEventPublisher eventPublisher,
            IEventStore<Core.Aggregates.Category> eventStore, 
            ISubscriber subscriber)
        {
            _createCategoryRequestValidator = createCategoryRequestValidator;
            _guidProvider = guidProvider;
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
            _subscriber = subscriber;
        }


        [FunctionName(nameof(CreateCategory))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Post, Route = "category")] CreateCategoryHttpRequest createCategoryRequest,
            [NotificationSubscriber] string subscriber,
            ILogger log)
        {
            var validationResult = await _createCategoryRequestValidator.ValidateAsync(createCategoryRequest);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }

            var id = _guidProvider.GenerateGuid();
            var correlationId = id;
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
                log.LogErrors(nameof(CreateCategory), saveEventResult.Errors);
                return new BadRequestResult();
            }

            await _eventPublisher.PublishEvent(categoryAddedEvent);

            return new AcceptedWithCorrelationIdHeaderResult(correlationId);
        }
    }
}
