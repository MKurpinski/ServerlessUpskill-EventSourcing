using Application.RequestMappers.Dtos;
using FluentValidation;
using Upskill.Infrastructure;

namespace Application.RequestMappers.Validators
{
    public class FinishedSchoolDtoValidator : AbstractValidator<FinishedSchoolDto>
    {
        public FinishedSchoolDtoValidator(IDateTimeProvider dateTimeProvider)
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.StartDate)
                .LessThan(dateTimeProvider.GetCurrentDateTime());
        }
    }
}
