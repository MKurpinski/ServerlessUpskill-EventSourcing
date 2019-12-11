using System;
using System.Collections.Generic;
using System.Text;

namespace Application.RequestMappers.Dtos
{
    public class WorkExperienceDto
    {
        public string CompanyName { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
    }
}
