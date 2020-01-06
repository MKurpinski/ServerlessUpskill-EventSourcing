using System;

namespace Application.Core.Events.ApplicationChangedEvent
{
    public class FinishedSchool
    {
        public string Name { get; }
        public DateTime StartDate { get; }
        public DateTime? FinishDate { get; }

        public FinishedSchool(string name, DateTime startDate, DateTime? finishDate)
        {
            Name = name;
            StartDate = startDate;
            FinishDate = finishDate;
        }
    }
}
