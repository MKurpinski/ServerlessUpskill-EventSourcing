using System;

namespace Application.RequestMappers.Dtos
{
    public class FinishedSchoolDto
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
    }
}
