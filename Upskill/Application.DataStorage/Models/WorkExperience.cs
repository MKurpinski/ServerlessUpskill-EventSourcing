using System;

namespace Application.DataStorage.Models
{
    public class WorkExperience
    {
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }

        public WorkExperience(string companyName, string position, DateTime startDate, DateTime? finishDate)
        {
            CompanyName = companyName;
            Position = position;
            StartDate = startDate;
            FinishDate = finishDate;
        }
    }
}
