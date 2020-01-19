using System;
using System.Collections.Generic;
using Application.Core.Events.CreateApplicationProcessStarted;
using Newtonsoft.Json;

namespace Application.Core.Events
{
    public class ApplicationCreatedEvent : CreateApplicationProcessStartedEvent
    {
        [JsonConstructor]
        public ApplicationCreatedEvent(
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
            : base(
                id,
                creationTime, 
                photoUri, 
                cvUri,
                category,
                firstName,
                lastName,
                educationLevel,
                address, 
                finishedSchools, 
                confirmedSkills,
                workExperiences,
                correlationId)
        {
        }

        public ApplicationCreatedEvent(
            CreateApplicationProcessStartedEvent processStarted)
            : this(
                processStarted.Id,
                processStarted.CreationTime,
                processStarted.PhotoUri,
                processStarted.CvUri,
                processStarted.Category,
                processStarted.FirstName,
                processStarted.LastName,
                processStarted.EducationLevel,
                processStarted.Address,
                processStarted.FinishedSchools,
                processStarted.ConfirmedSkills,
                processStarted.WorkExperiences,
                processStarted.CorrelationId)
        {
        }
    }
}
