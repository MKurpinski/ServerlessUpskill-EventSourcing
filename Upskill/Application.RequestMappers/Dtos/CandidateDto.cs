using System.Collections.Generic;

namespace Application.RequestMappers.Dtos
{
    public class CandidateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Category { get; set; }
        public AddressDto Address { get; set; }
        public string EducationLevel { get; set; }
        public IReadOnlyCollection<FinishedSchoolDto> FinishedSchools { get; set; }
        public IReadOnlyCollection<ConfirmedSkillDto> ConfirmedSkills { get; set; }
        public IReadOnlyCollection<WorkExperienceDto> WorkExperiences { get; set; }
    }
}
