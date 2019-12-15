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
            var client = await SearchIndexClientProvider.Get<T>();

            var searchParameters = new SearchParameters
            {
                 QueryType = QueryType.Full,
                 SearchFields = new List<string> { nameof(ISearchable.Id).ToLowerInvariant() }
            };

            var searchResults = await client.Documents.SearchAsync<T>(id, searchParameters);

            return searchResults;
        }
    }
}
