using Application.Api.CustomHttpRequests;
using FluentValidation;

namespace Application.Api.Validators
{
    public class SimpleApplicationSearchHttpRequestValidator : AbstractValidator<SimpleApplicationSearchHttpRequest>
    {
        public SimpleApplicationSearchHttpRequestValidator()
        {
            RuleFor(x => x.Query)
                .NotEmpty();

            RuleFor(x => x.Skip)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Take)
                .GreaterThan(0);
        }
    }
}
