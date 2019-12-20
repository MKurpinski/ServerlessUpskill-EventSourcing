using Category.Api.CustomHttpRequests;
using Category.DataStorage.Repositories;
using FluentValidation;

namespace Category.Api.Validators
{
    public class ModifyCategoryHttpRequestValidator : AbstractValidator<IModifyCategoryHttpRequest>
    {
        public ModifyCategoryHttpRequestValidator(ICategoryRepository categoryRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .CustomAsync(async (name, context, cancellationToken) =>
                {
                    var existingCategoryResult = await categoryRepository.GetByName(name);

                    if (existingCategoryResult.Success)
                    {
                        context.AddFailure(nameof(IModifyCategoryHttpRequest.Name), "Name currently used.");
                    }
                });

            RuleFor(x => x.Description)
                .NotEmpty();

            RuleFor(x => x.SortOrder)
                .GreaterThan(0);
        }
    }
}
