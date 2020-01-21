using System;
using Microsoft.Azure.Search;
using Microsoft.Extensions.Options;
using Upskill.Search.Options;

namespace Upskill.Search.Providers
{
    public class SearchServiceClientProvider : ISearchServiceClientProvider
    {
        private readonly SearchOptions _searchOptions;
        private readonly Lazy<ISearchServiceClient> _lazySearchServiceClient;

        public SearchServiceClientProvider(IOptions<SearchOptions> searchOptionsAccessor)
        {
            _searchOptions = searchOptionsAccessor.Value;
            _lazySearchServiceClient = new Lazy<ISearchServiceClient>(this.GetInternal);
        }

        public ISearchServiceClient Get()
        {
            return _lazySearchServiceClient.Value;
        }

        private ISearchServiceClient GetInternal()
        {
            return new SearchServiceClient(_searchOptions.SearchServiceName, new SearchCredentials(_searchOptions.SearchServiceAdminKey));
        }
    }
}
