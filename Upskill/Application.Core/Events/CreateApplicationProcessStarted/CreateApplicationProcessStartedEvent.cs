using System;
using System.Collections.Generic;
using Upskill.Events;

namespace Application.Core.Events.CreateApplicationProcessStarted
{
    public class CreateApplicationProcessStartedEvent : BaseEvent
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

        public CreateApplicationProcessStartedEvent(
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
            IReadOnlyCollection<WorkExperience> workExperiences,
            string correlationId) 
            : base(correlationId)
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
