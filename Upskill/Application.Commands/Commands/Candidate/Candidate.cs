using System.Collections.Generic;

namespace Application.Commands.Commands.Candidate
{
    public class Candidate
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Category { get; }

        public Address Address { get; }
        public string EducationLevel { get; }
        public IReadOnlyCollection<FinishedSchool> FinishedSchools { get; }
        public IReadOnlyCollection<ConfirmedSkill> ConfirmedSkills { get; }
        public IReadOnlyCollection<WorkExperience> WorkExperiences { get; }

        public Candidate(
            string firstName,
            string lastName,
            string category,
            string educationLevel,
            Address address,
            IReadOnlyCollection<FinishedSchool> finishedSchools,
            IReadOnlyCollection<ConfirmedSkill> confirmedSkills,
            IReadOnlyCollection<WorkExperience> workExperiences)
        {
            FirstName = firstName;
            LastName = lastName;
            Category = category;
            EducationLevel = educationLevel;
            Address = address;
            FinishedSchools = finishedSchools;
            ConfirmedSkills = confirmedSkills;
            WorkExperiences = workExperiences;
        }
    }
}
