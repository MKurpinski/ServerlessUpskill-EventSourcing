using System;
using Microsoft.Azure.Search;

namespace Application.Search.Model
{
    public class SearchableConfirmedSkill
    {
        [IsFacetable, IsSearchable, IsFilterable]
        public string Name { get; set; }
        public DateTime DateOfAchievement { get; set; }
    }
}
