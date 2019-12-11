using System;
using System.Collections.Generic;

namespace Application.DataStorage.Model
{
    public class Application
    {
        public string Id { get; set; }
        public DateTime CreationTime { get; }
        public string PhotoUri { get; }
        public string CvUri { get; }
        public string Category { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public Address Address { get; }
        public string EducationLevel { get; }
        public IReadOnlyCollection<FinishedSchool> FinishedSchools { get; }
        public IReadOnlyCollection<ConfirmedSkill> ConfirmedSkills { get; }
        public IReadOnlyCollection<WorkExperience> WorkExperiences { get; }

        public Application(
            string id,
            DateTime creationTime,
            string photoUri,
            string cvUri,
            string category,
            string firstName,
            string lastName,
            string educationLevel,
            Address address,
            IReadOnlyCollection<FinishedSchool> finishedSchools,
            IReadOnlyCollection<ConfirmedSkill> confirmedSkills,
            IReadOnlyCollection<WorkExperience> workExperiences)
        {
            Id = id;
            CreationTime = creationTime;
            PhotoUri = photoUri;
            CvUri = cvUri;
            Category = category;
            FirstName = firstName;
            LastName = lastName;
            EducationLevel = educationLevel;
            Address = address;
            FinishedSchools = finishedSchools;
            ConfirmedSkills = confirmedSkills;
            WorkExperiences = workExperiences;
        }
    }
}
