using System;
using System.Collections.Generic;

namespace Application.Search.Dtos
{
    public class ApplicationDto
    {
        public string Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string PhotoUri { get; set; }
        public string CvUri { get; set; }
        public string Category { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AddressDto Address { get; set; }
        public string EducationLevel { get; set; }
        public IReadOnlyCollection<FinishedSchoolDto> FinishedSchools { get; set; }
        public IReadOnlyCollection<ConfirmedSkillDto> ConfirmedSkills { get; set; }
        public IReadOnlyCollection<WorkExperienceDto> WorkExperiences { get; set; }
    }
}
