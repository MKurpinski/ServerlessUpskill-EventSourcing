using Application.RequestMappers.Dtos;
using FluentValidation;

namespace Application.RequestMappers.Validators
{
    public class CandidateDtoValidator: AbstractValidator<CandidateDto>
    {
        public CandidateDtoValidator(
            IValidator<AddressDto> addressDtoValidator,
            IValidator<ConfirmedSkillDto> confirmedSkillDtoValidator,
            IValidator<WorkExperienceDto> workExperienceDtoValidator,
            IValidator<FinishedSchoolDto> finishedSchoolValidator)
        {
            RuleFor(x => x.Category)
                .NotEmpty();

            RuleFor(x => x.EducationLevel)
                .NotEmpty();

            RuleFor(x => x.FirstName)
                .NotEmpty();

            RuleFor(x => x.LastName)
                .NotEmpty();

            RuleFor(x => x.Address)
                .NotNull()
                .SetValidator(addressDtoValidator);

            RuleForEach(x => x.ConfirmedSkills)
                .NotNull()
                .SetValidator(confirmedSkillDtoValidator);

            RuleForEach(x => x.WorkExperiences)
                .NotNull()
                .SetValidator(workExperienceDtoValidator);

            RuleForEach(x => x.FinishedSchools)
                .NotNull()
                .SetValidator(finishedSchoolValidator);
        }
    }
}
