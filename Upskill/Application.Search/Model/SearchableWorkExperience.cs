using System;
using Microsoft.Azure.Search;

namespace Application.Search.Model
{
    public class SearchableWorkExperience
    {
        [IsFacetable, IsSearchable, IsFilterable]

        public string CompanyName { get; set; }

        [IsFacetable, IsSearchable, IsFilterable]
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
    }
}
