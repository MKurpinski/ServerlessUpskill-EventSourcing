using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Upskill.Search.Enums;
using Upskill.Search.Models;
using Upskill.Search.Resolvers;

namespace Upskill.Search.Providers
{
    public class SearchIndexClientProvider : ISearchIndexClientProvider
    {
        private readonly ISearchServiceClient _searchServiceClient;
        private readonly IIndexNameResolver _indexNameResolver;

        public SearchIndexClientProvider(
            ISearchServiceClientProvider searchServiceClientProvider,
            IIndexNameResolver indexNameResolver)
        {
            _indexNameResolver = indexNameResolver;
            _searchServiceClient = searchServiceClientProvider.Get();
        }

        public async Task<ISearchIndexClient> Get<T>(IndexType indexType = IndexType.Active) where T : ISearchable
        {
            var indexName = await _indexNameResolver.ResolveIndexName<T>(indexType);
            return _searchServiceClient.Indexes.GetClient(indexName);
        }
    }
}