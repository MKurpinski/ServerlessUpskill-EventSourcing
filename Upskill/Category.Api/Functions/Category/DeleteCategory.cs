using System.Threading.Tasks;
using Category.Core.Events;
using Microsoft.AspNetCore.Http;
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
using Upskill.Telemetry.CorrelationInitializers;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Category.Api.Functions.Category
{
    public class DeleteCategory
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore<Core.Aggregates.Category> _eventStore;
        private readonly IGuidProvider _guidProvider;
        private readonly ISubscriber _subscriber;
        private readonly ICorrelationInitializer _correlationInitializer;

        public DeleteCategory(
            IEventPublisher eventPublisher,
            IEventStore<Core.Aggregates.Category> eventStore,
            IGuidProvider guidProvider, 
            ISubscriber subscriber,
            ICorrelationInitializer correlationInitializer)
        {
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
            _guidProvider = guidProvider;
            _subscriber = subscriber;
            _correlationInitializer = correlationInitializer;
        }

        [FunctionName(nameof(DeleteCategory))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Delete, Route = "category/{id:guid}")] HttpRequest req,
            [NotificationSubscriber] string subscriber,
            string id,
            ILogger log)
        {
            var correlationId = _guidProvider.GenerateGuid();
            _correlationInitializer.Initialize(correlationId);
            log.LogProgress(OperationStatus.Started, "Deleting category process started", correlationId);

            var categoryDeletedEvent = new DeleteCategoryProcessStartedEvent(id, correlationId);

            await _subscriber.Register(correlationId, subscriber);
            var saveEventResult = await _eventStore.AppendEvent(id, categoryDeletedEvent);

            if (!saveEventResult.Success)
            {
                log.LogFailedOperation(OperationStatus.Failed, "Deleting category process failed", saveEventResult.Errors, correlationId);
                return new BadRequestResult();
            }

            log.LogProgress(OperationStatus.InProgress, "Request accepted to further processing.", correlationId);
            await _eventPublisher.PublishEvent(categoryDeletedEvent);
            log.LogProgress(OperationStatus.InProgress, $"{nameof(DeleteCategoryProcessStartedEvent)} published", correlationId);

            return new AcceptedWithCorrelationIdHeaderResult(correlationId);
        }
    }
}
