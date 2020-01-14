using System.Threading.Tasks;
using Category.Api.Commands;
using Category.Api.CustomHttpRequests;
using Category.Core.Events.Internal;
using Category.EventStore.Facades;
using FluentValidation;
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
    public class UpdateCategory
    {
        private readonly IValidator<UpdateCategoryCommand> _updateCommandValidator;
        private readonly IGuidProvider _guidProvider;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStoreFacade _eventStore;

        public UpdateCategory(
            IValidator<UpdateCategoryCommand> updateCommandValidator,
            IEventPublisher eventPublisher,
            IEventStoreFacade eventStore, IGuidProvider guidProvider)
        {
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
            _guidProvider = guidProvider;
            _updateCommandValidator = updateCommandValidator;
        }

        [FunctionName(nameof(UpdateCategory))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Put, Route = "category/{id:guid}")] UpdateCategoryHttpRequest updateCategoryRequest,
            string id,
            ILogger log)
        {
            var correlationId = _guidProvider.GenerateGuid(); 
            var validationResult = await _updateCommandValidator.ValidateAsync(new UpdateCategoryCommand(id, updateCategoryRequest));

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }

            var categoryChangedEvent = new UpdateCategoryProcessStartedEvent(
                id,
                updateCategoryRequest.Name,
                updateCategoryRequest.Description,
                updateCategoryRequest.SortOrder,
                correlationId);

            var saveEventResult = await _eventStore.AppendEvent(id, categoryChangedEvent);

            if (!saveEventResult.Success)
            {
                log.LogErrors(nameof(UpdateCategory), saveEventResult.Errors);
                return new BadRequestResult();
            }

            await _eventPublisher.PublishEvent(categoryChangedEvent);

            return new AcceptedWithCorrelationIdHeaderResult(correlationId);
        }
    }
}
