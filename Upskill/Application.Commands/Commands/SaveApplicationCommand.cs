using System;
using System.Collections.Generic;
using Application.Commands.Commands.Candidate;

namespace Application.Commands.Commands
{
    public class SaveApplicationCommand
    {
        public DateTime CreationTime { get; }
        public string PhotoId { get; }
        public string CvId { get; }
        public string Category { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public Address Address { get; }
        public string EducationLevel { get; }
        public IReadOnlyCollection<FinishedSchool> FinishedSchools { get; }
        public IReadOnlyCollection<ConfirmedSkill> ConfirmedSkills { get; }
        public IReadOnlyCollection<WorkExperience> WorkExperiences { get; }

        public SaveApplicationCommand(
            string firstName,
            string lastName,
            string photoId,
            string cvId,
            string category,
            DateTime creationTime, 
            string educationLevel,
            Address address,
            IReadOnlyCollection<FinishedSchool> finishedSchools,
            IReadOnlyCollection<ConfirmedSkill> confirmedSkills,
            IReadOnlyCollection<WorkExperience> workExperiences)
        {
            CreationTime = creationTime;
            EducationLevel = educationLevel;
            Address = address;
            FinishedSchools = finishedSchools;
            ConfirmedSkills = confirmedSkills;
            WorkExperiences = workExperiences;
            PhotoId = photoId;
            CvId = cvId;
            Category = category;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
