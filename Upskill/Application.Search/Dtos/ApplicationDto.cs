using System;
using System.Collections.Generic;

namespace Application.Search.Dtos
{
    public class ApplicationDto: SimpleApplicationDto
    {
        public DateTime CreationTime { get; set; }
        public AddressDto Address { get; set; }
        public IReadOnlyCollection<FinishedSchoolDto> FinishedSchools { get; set; }
        public IReadOnlyCollection<ConfirmedSkillDto> ConfirmedSkills { get; set; }
        public IReadOnlyCollection<WorkExperienceDto> WorkExperiences { get; set; }
    }
}
