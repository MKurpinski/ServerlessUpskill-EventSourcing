using Category.Api.CustomHttpRequests;
using FluentValidation;

namespace Category.Api.Validators
{
    public class CreateCategoryHttpRequestValidator : AbstractValidator<CreateCategoryHttpRequest>
    {
        public CreateCategoryHttpRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.Description)
                .NotEmpty();

            RuleFor(x => x.SortOrder)
                .GreaterThan(0);
        }
    }
}
