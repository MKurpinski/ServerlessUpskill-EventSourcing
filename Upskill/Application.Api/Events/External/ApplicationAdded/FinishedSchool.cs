﻿using System;

namespace Application.Api.Events.External.ApplicationAdded
{
    public class FinishedSchool
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }

        public FinishedSchool(string name, DateTime startDate, DateTime? finishDate)
        {
            Name = name;
            StartDate = startDate;
            FinishDate = finishDate;
        }
    }
}
