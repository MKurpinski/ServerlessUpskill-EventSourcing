using Application.RequestMappers.Dtos;
using FluentValidation;
using Upskill.Infrastructure;

namespace Application.RequestMappers.Validators
{
    public class ConfirmedSkillDtoValidator : AbstractValidator<ConfirmedSkillDto>
    {
        public ConfirmedSkillDtoValidator(IDateTimeProvider dateTimeProvider)
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.DateOfAchievement)
                .LessThan(dateTimeProvider.GetCurrentDateTime());
        }
    }
}
