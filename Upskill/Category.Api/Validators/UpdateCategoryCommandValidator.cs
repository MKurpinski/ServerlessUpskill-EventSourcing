using Category.Api.Commands;
using Category.Api.CustomHttpRequests;
using Category.DataStorage.Repositories;
using FluentValidation;

namespace Category.Api.Validators
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator(ICategoryRepository categoryRepository)
        {
            RuleFor(x => x.Request.Name)
                .NotEmpty()
                .MustAsync(async (x, name, cancellationToken) =>
                {
                    var existingCategoryResult = await categoryRepository.GetByName(x.Request.Name);

                    if (existingCategoryResult.Success && !x.Id.Equals(existingCategoryResult.Value.Id))
                    {
                        return false;
                    }

                    return true;
                }).WithMessage("Name already used");


            RuleFor(x => x.Request.Description)
                .NotEmpty();

            RuleFor(x => x.Request.SortOrder)
                .GreaterThan(0);
        }
    }
}
