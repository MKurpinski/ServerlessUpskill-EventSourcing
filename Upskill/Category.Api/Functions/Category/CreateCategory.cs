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
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Category.Api.Functions.Category
{
    public class CreateCategory
    {
        private readonly IGuidProvider _guidProvider;
        private readonly IValidator<CreateCategoryHttpRequest> _createCategoryRequestValidator;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore _eventStore;

        public CreateCategory(
            IValidator<CreateCategoryHttpRequest> createCategoryRequestValidator,
            IGuidProvider guidProvider,
            IEventPublisher eventPublisher,
            IEventStore eventStore)
        {
            _createCategoryRequestValidator = createCategoryRequestValidator;
            _guidProvider = guidProvider;
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
        }


        [FunctionName(nameof(CreateCategory))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Post, Route = "category")] CreateCategoryHttpRequest createCategoryRequest,
            ILogger log)
        {
            var validationResult = await _createCategoryRequestValidator.ValidateAsync(createCategoryRequest);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }

            var id = _guidProvider.GenerateGuid().ToString("N");

            var categoryAddedEvent = new InternalCategoryAddedEvent(
                id,
                createCategoryRequest.Name,
                createCategoryRequest.Description,
                createCategoryRequest.SortOrder);

            var saveEventResult = await _eventStore.AppendEvent(id, categoryAddedEvent);

            if (!saveEventResult.Success)
            {
                log.LogErrors(nameof(CreateCategory), saveEventResult.Errors);
                return new BadRequestResult();
            }

            await _eventPublisher.PublishEvent(categoryAddedEvent);

            return new AcceptedWithCorrelationIdHeaderResult(id);
        }
    }
}
