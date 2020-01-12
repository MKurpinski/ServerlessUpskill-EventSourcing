using System.Threading.Tasks;
using Category.Core.Events;
using Category.Core.Events.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.EventStore;
using Upskill.FunctionUtils.Results;
using Upskill.Infrastructure.Extensions;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Category.Api.Functions.Category
{
    public class DeleteCategory
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore _eventStore;


        public DeleteCategory(
            IEventPublisher eventPublisher,
            IEventStore eventStore)
        {
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
        }

        [FunctionName(nameof(DeleteCategory))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Delete, Route = "category/{id:guid}")] HttpRequest req,
            string id,
            ILogger log)
        {
            var categoryDeletedEvent = new InternalCategoryDeletedEvent(id);

            var saveEventResult = await _eventStore.AppendEvent(id, categoryDeletedEvent);

            if (!saveEventResult.Success)
            {
                log.LogErrors(nameof(DeleteCategory), saveEventResult.Errors);
                return new BadRequestResult();
            }

            await _eventPublisher.PublishEvent(new InternalCategoryDeletedEvent(id));
            return new AcceptedWithCorrelationIdHeaderResult(id);
        }
    }
}
