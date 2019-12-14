using System;

namespace Application.Search.Dtos
{
    public class WorkExperienceDto
    {
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
    }
}
