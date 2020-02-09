using Category.Api.CustomHttpRequests;
using FluentValidation;

namespace Category.Api.Validators
{
    public class GetCategoriesHttpRequestValidator : AbstractValidator<GetCategoriesHttpRequest>
    {
        public GetCategoriesHttpRequestValidator()
        {
            RuleFor(x => x.Skip)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Take)
                .GreaterThan(0);
        }
    }
}
