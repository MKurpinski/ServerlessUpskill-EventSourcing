using System.Threading.Tasks;
using Category.Api.Commands;
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
using Upskill.Infrastructure.Extensions;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Category.Api.Functions.Category
{
    public class UpdateCategory
    {
        private readonly IValidator<UpdateCategoryCommand> _updateCommandValidator;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore _eventStore;


        public UpdateCategory(
            IValidator<UpdateCategoryCommand> updateCommandValidator,
            IEventPublisher eventPublisher,
            IEventStore eventStore)
        {
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
            _updateCommandValidator = updateCommandValidator;
        }

        [FunctionName(nameof(UpdateCategory))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Put, Route = "category/{id:guid}")] UpdateCategoryHttpRequest updateCategoryRequest,
            string id,
            ILogger log)
        {
            var validationResult = await _updateCommandValidator.ValidateAsync(new UpdateCategoryCommand(id, updateCategoryRequest));

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }

            var categoryChangedEvent = new InternalCategoryChangedEvent(
                id,
                updateCategoryRequest.Name,
                updateCategoryRequest.Description,
                updateCategoryRequest.SortOrder);

            var saveEventResult = await _eventStore.AppendEvent(id, categoryChangedEvent);

            if (!saveEventResult.Success)
            {
                log.LogErrors(nameof(UpdateCategory), saveEventResult.Errors);
                return new BadRequestResult();
            }

            await _eventPublisher.PublishEvent(categoryChangedEvent);

            return new AcceptedWithCorrelationIdHeaderResult(id);
        }
    }
}
