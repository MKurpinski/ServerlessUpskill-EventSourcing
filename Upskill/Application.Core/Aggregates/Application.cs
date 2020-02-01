using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core.Events;
using Application.Core.ValueObjects;
using Upskill.Events;
using Upskill.EventStore.Models;

namespace Application.Core.Aggregates
{
    public class Application : IAggregateRoot, IBuildBy<ApplicationCreatedEvent>, IBuildBy<ApplicationCategoryNameChangedEvent>
    {
        public string Id { get; private set; }
        public string PhotoUri { get; private set; }
        public string CvUri { get; private set; }
        public string Category { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string EducationLevel { get; private set; }
        public DateTime CreationTime { get; private set; }
        public Address Address { get; private set; }
        public IReadOnlyCollection<FinishedSchool> FinishedSchools { get; private set; }
        public IReadOnlyCollection<ConfirmedSkill> ConfirmedSkills { get; private set; }
        public IReadOnlyCollection<WorkExperience> WorkExperiences { get; private set; }

        public void ApplyEvent(ApplicationCategoryNameChangedEvent applicationCategoryNameChangedEvent)
        {
            this.Category = applicationCategoryNameChangedEvent.NewCategoryName;
        }

        public void ApplyEvent(ApplicationCreatedEvent applicationCreatedEvent)
        {
            this.Id = applicationCreatedEvent.Id;
            this.PhotoUri = applicationCreatedEvent.PhotoUri;
            this.CvUri = applicationCreatedEvent.CvUri;
            this.Category = applicationCreatedEvent.Category;
            this.FirstName = applicationCreatedEvent.FirstName;
            this.LastName = applicationCreatedEvent.LastName;
            this.EducationLevel = applicationCreatedEvent.EducationLevel;
            this.CreationTime = applicationCreatedEvent.CreationTime;
            this.Address = new Address(applicationCreatedEvent.Address.City, applicationCreatedEvent.Address.Country);
            this.FinishedSchools = applicationCreatedEvent.FinishedSchools
                .Select(x => new FinishedSchool(x.Name, x.StartDate, x.FinishDate)).ToList();
            this.ConfirmedSkills = applicationCreatedEvent.ConfirmedSkills
                .Select(x => new ConfirmedSkill(x.Name, x.DateOfAchievement)).ToList();
            this.WorkExperiences = applicationCreatedEvent.WorkExperiences
                .Select(x => new WorkExperience(x.CompanyName, x.Position, x.StartDate, x.FinishDate)).ToList();
        }
    }
}
