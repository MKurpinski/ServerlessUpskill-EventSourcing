using System.Threading.Tasks;
using Category.Core.Events;
using Category.Core.Events.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.FunctionUtils.Results;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Category.Api.Functions.Category
{
    public class DeleteCategory
    {
        private readonly IEventPublisher _eventPublisher;

        public DeleteCategory(
            IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        [FunctionName(nameof(DeleteCategory))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Delete, Route = "category/{id:guid}")] HttpRequest req,
            string id)
        {
            // save event

            await _eventPublisher.PublishEvent(new InternalCategoryDeletedEvent(id));
            return new AcceptedWithCorrelationIdHeaderResult(id);
        }
    }
}
