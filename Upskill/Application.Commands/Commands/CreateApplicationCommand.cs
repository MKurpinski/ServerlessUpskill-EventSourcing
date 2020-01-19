using System;
using System.Collections.Generic;
using Application.Commands.Commands.Candidate;

namespace Application.Commands.Commands
{
    public class CreateApplicationCommand
    {
        public string Id { get; }
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
        public string CorrelationId { get; }

        public CreateApplicationCommand(
            string id,
            string firstName,
            string lastName,
            string photoUri,
            string cvUri,
            string category,
            DateTime creationTime, 
            string educationLevel,
            Address address,
            IReadOnlyCollection<FinishedSchool> finishedSchools,
            IReadOnlyCollection<ConfirmedSkill> confirmedSkills,
            IReadOnlyCollection<WorkExperience> workExperiences,
            string correlationId)
        {
            Id = id;
            CreationTime = creationTime;
            EducationLevel = educationLevel;
            Address = address;
            FinishedSchools = finishedSchools;
            ConfirmedSkills = confirmedSkills;
            WorkExperiences = workExperiences;
            CorrelationId = correlationId;
            PhotoUri = photoUri;
            CvUri = cvUri;
            Category = category;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
