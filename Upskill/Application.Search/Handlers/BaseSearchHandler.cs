using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Search.Models;
using Application.Search.Providers;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace Application.Search.Handlers
{
    public abstract class BaseSearchHandler<T> where T : ISearchable
    {
        protected readonly ISearchIndexClientProvider SearchIndexClientProvider;

        protected BaseSearchHandler(ISearchIndexClientProvider searchIndexClientProvider)
        {
            SearchIndexClientProvider = searchIndexClientProvider;
        }

        protected async Task<DocumentSearchResult<T>> WildcardSearch(string searchPhrase, SearchParameters searchParameters)
        {
            var wildcardSearchQuery = $"/.*{searchPhrase}.*/";
            var client = await SearchIndexClientProvider.Get<T>();

            searchParameters.QueryType = QueryType.Full;

            var searchResults = await client.Documents.SearchAsync<T>(wildcardSearchQuery, searchParameters);

            return searchResults;
        }

        protected async Task<DocumentSearchResult<T>> GetById(string id)
        {
            return await GetByField(nameof(ISearchable.Id), id);
        }

        protected async Task<DocumentSearchResult<T>> GetByField(string fieldName, string value, int? skip = null, int? top = null)
        {
            var client = await SearchIndexClientProvider.Get<T>();

            var searchParameters = new SearchParameters
            {
                QueryType = QueryType.Full,
                Skip = skip,
                Top = top,
                IncludeTotalResultCount = true,
                SearchFields = new List<string> { fieldName.ToLowerInvariant() }
            };

            var searchResults = await client.Documents.SearchAsync<T>(value, searchParameters);

            return searchResults;
        }
    }
}
