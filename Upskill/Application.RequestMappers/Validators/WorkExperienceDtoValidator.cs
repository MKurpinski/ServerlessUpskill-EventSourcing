using Application.RequestMappers.Dtos;
using FluentValidation;
using Upskill.Infrastructure;

namespace Application.RequestMappers.Validators
{
    public class WorkExperienceDtoValidator : AbstractValidator<WorkExperienceDto>
    {
        public WorkExperienceDtoValidator(IDateTimeProvider dateTimeProvider)
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty();

            RuleFor(x => x.Position)
                .NotEmpty();

            RuleFor(x => x.StartDate)
                .LessThan(dateTimeProvider.GetCurrentDateTime());
        }
    }
}
