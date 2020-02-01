using Microsoft.Azure.Search;
using Upskill.Search.Models;

namespace Category.Search.Models
{
    public class SearchableCategory : ISearchable
    {
        [System.ComponentModel.DataAnnotations.Key]
        [IsSearchable]
        public string Id { get; set; }
        [IsSearchable]
        public string Name { get; set; }
        [IsSearchable]
        public string Description { get; set; }
        [IsSortable]
        public int SortOrder { get; set; }

        public SearchableCategory()
        {

        }

        public SearchableCategory(string id)
        {
            Id = id;
        }

        public SearchableCategory(string id, string name, string description, int sortOrder)
        {
            Id = id;
            Name = name;
            Description = description;
            SortOrder = sortOrder;
        }
    }
}
