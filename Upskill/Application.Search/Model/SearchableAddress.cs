using Microsoft.Azure.Search;

namespace Application.Search.Model
{
    public class SearchableAddress
    {
        [IsFacetable, IsSearchable, IsFilterable]
        public string City { get; set; }

        [IsSortable, IsFacetable, IsSearchable, IsFilterable]
        public string Country { get; set; }
    }
}
