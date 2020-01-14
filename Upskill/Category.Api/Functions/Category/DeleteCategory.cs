using System.Threading.Tasks;
using Category.Core.Events.Internal;
using Category.EventStore.Facades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.FunctionUtils.Results;
using Upskill.Infrastructure;
using Upskill.Infrastructure.Extensions;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Category.Api.Functions.Category
{
    public class DeleteCategory
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStoreFacade _eventStore;
        private readonly IGuidProvider _guidProvider;

        public DeleteCategory(
            IEventPublisher eventPublisher,
            IEventStoreFacade eventStore, IGuidProvider guidProvider)
        {
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
            _guidProvider = guidProvider;
        }

        [FunctionName(nameof(DeleteCategory))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Delete, Route = "category/{id:guid}")] HttpRequest req,
            string id,
            ILogger log)
        {
            var correlationId = _guidProvider.GenerateGuid();
            var categoryDeletedEvent = new InternalCategoryDeletedEvent(id, correlationId);

            var saveEventResult = await _eventStore.AppendEvent(id, categoryDeletedEvent);

            if (!saveEventResult.Success)
            {
                log.LogErrors(nameof(DeleteCategory), saveEventResult.Errors);
                return new BadRequestResult();
            }

            await _eventPublisher.PublishEvent(categoryDeletedEvent);
            return new AcceptedWithCorrelationIdHeaderResult(correlationId);
        }
    }
}
