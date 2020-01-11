using System;

namespace Application.Core.Events.ApplicationAddedEvent
{
    public class WorkExperience
    {
        public string CompanyName { get; }
        public string Position { get; }
        public DateTime StartDate { get; }
        public DateTime? FinishDate { get; }

        public WorkExperience(string companyName, string position, DateTime startDate, DateTime? finishDate)
        {
            CompanyName = companyName;
            Position = position;
            StartDate = startDate;
            FinishDate = finishDate;
        }
    }
}
