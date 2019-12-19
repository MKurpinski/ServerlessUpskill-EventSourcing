using System;
using System.Collections.Generic;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace Application.Search.Models
{
    [SerializePropertyNamesAsCamelCase]
    public class SearchableApplication : ISearchable
    {
        [System.ComponentModel.DataAnnotations.Key]
        [IsSearchable]
        public string Id { get; set; }

        [IsSortable, IsFilterable]
        public DateTime CreationTime { get; set; }

        public string PhotoUri { get; set; }

        public string CvUri { get; set; }

        [IsSortable, IsFacetable, IsSearchable, IsFilterable]
        public string Category { get; set; }

        [IsSearchable]
        public string FirstName { get; set; }

        [IsSearchable]
        public string LastName { get; set; }

        public SearchableAddress Address { get; set; }

        [IsSortable, IsFacetable, IsSearchable, IsFilterable]
        public string EducationLevel { get; set; }

        public IReadOnlyCollection<SearchableFinishedSchool> FinishedSchools { get; set; }

        public IReadOnlyCollection<SearchableConfirmedSkill> ConfirmedSkills { get; set; }

        public IReadOnlyCollection<SearchableWorkExperience> WorkExperiences { get; set; }
    }
}
