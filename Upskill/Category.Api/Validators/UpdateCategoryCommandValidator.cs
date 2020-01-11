using Category.Api.Commands;
using FluentValidation;

namespace Category.Api.Validators
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Request.Name)
                .NotEmpty();

            RuleFor(x => x.Request.Description)
                .NotEmpty();

            RuleFor(x => x.Request.SortOrder)
                .GreaterThan(0);
        }
    }
}
