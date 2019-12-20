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
using HttpMethods = Upskill.FunctionUtils.Constants.HttpMethods;

namespace Category.Api.Functions
{
    public class UpdateCategory
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IValidator<IModifyCategoryHttpRequest> _createCategoryRequestValidator;
        private readonly IEventPublisher _eventPublisher;

        public UpdateCategory(
            ICategoryRepository categoryRepository,
            IValidator<IModifyCategoryHttpRequest> createCategoryRequestValidator,
            IEventPublisher eventPublisher)
        {
            _categoryRepository = categoryRepository;
            _createCategoryRequestValidator = createCategoryRequestValidator;
            _eventPublisher = eventPublisher;
        }

        [FunctionName(nameof(UpdateCategory))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, HttpMethods.Put, Route = "category/{id:guid}")] UpdateCategoryHttpRequest updateCategoryRequest,
            string id)
        {
            var existingCategoryResult = await _categoryRepository.GetById(id);

            if (!existingCategoryResult.Success)
            {
                return new NotFoundResult();
            }

            var validationResult = await _createCategoryRequestValidator.ValidateAsync(updateCategoryRequest);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors);
            }

            var categoryToUpdate = new CategoryDto(
                id,
                updateCategoryRequest.Name,
                updateCategoryRequest.Description,
                updateCategoryRequest.SortOrder);

            var updateResult = await _categoryRepository.Update(categoryToUpdate);

            if (!updateResult.Success)
            {
                return new BadRequestResult();
            }

            await _eventPublisher.PublishEvent(this.BuildCategoryChangedEvent(categoryToUpdate));

            return new NoContentResult();
        }

        private CategoryChangedEvent BuildCategoryChangedEvent(CategoryDto dto)
        {
            return new CategoryChangedEvent(dto.Id, dto.Name, dto.Description, dto.SortOrder);
        }
    }
}
