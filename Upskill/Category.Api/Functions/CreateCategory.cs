using System.Threading.Tasks;
using Category.Api.CustomHttpRequests;
using Category.Api.Events;
using Category.DataStorage.Dtos;
using Category.DataStorage.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Upskill.EventPublisher.Publishers;
using Upskill.Infrastructure;
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Category.Api.Functions
{
    public class CreateCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IGuidProvider _guidProvider;
        private readonly IValidator<IModifyCategoryHttpRequest> _createCategoryRequestValidator;
        private readonly IEventPublisher _eventPublisher;

        public CreateCategory(
            ICategoryRepository categoryRepository,
            IValidator<IModifyCategoryHttpRequest> createCategoryRequestValidator,
            IGuidProvider guidProvider,
            IEventPublisher eventPublisher)
        {
            _categoryRepository = categoryRepository;
            _createCategoryRequestValidator = createCategoryRequestValidator;
            _guidProvider = guidProvider;
            _eventPublisher = eventPublisher;
        }


        [FunctionName(nameof(CreateCategory))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Post, Route = "category")] CreateCategoryHttpRequest createCategoryRequest)
        {
            var validationResult = await _createCategoryRequestValidator.ValidateAsync(createCategoryRequest);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }

            var id = _guidProvider.GenerateGuid().ToString("N");

            var categoryToAdd = new CategoryDto(
                id,
                createCategoryRequest.Name,
                createCategoryRequest.Description,
                createCategoryRequest.SortOrder);

            var createResult = await _categoryRepository.Create(categoryToAdd);

            if (!createResult.Success)
            {
                return new BadRequestResult();
            }

            await _eventPublisher.PublishEvent(this.BuildCategoryChangedEvent(categoryToAdd));
            return new CreatedResult(id, categoryToAdd);
        }

        private CategoryChangedEvent BuildCategoryChangedEvent(CategoryDto dto)
        {
            return new CategoryChangedEvent(dto.Id, dto.Name, dto.Description, dto.SortOrder);
        }
    }
}
