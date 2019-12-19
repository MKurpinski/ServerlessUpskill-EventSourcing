using System.Threading.Tasks;
using Application.Search.Models;
using Application.Search.Resolvers;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace Application.Search.Providers
{
    public class SearchIndexClientProvider : ISearchIndexClientProvider
    {
        private readonly ISearchServiceClient _searchServiceClient;
        private readonly ICurrentIndexNameResolver _currentIndexNameResolver;

        public SearchIndexClientProvider(
            ISearchServiceClientProvider searchServiceClientProvider,
            ICurrentIndexNameResolver currentIndexNameResolver)
        {
            _currentIndexNameResolver = currentIndexNameResolver;
            _searchServiceClient = searchServiceClientProvider.Get();
        }

        public async Task<ISearchIndexClient> Get<T>() where T : ISearchable
        {
            var indexName = await _currentIndexNameResolver.ResolveCurrentIndexName<T>();
            await CreateIndexIfNotExist<T>(indexName);
            return _searchServiceClient.Indexes.GetClient(indexName);
        }

        private async Task CreateIndexIfNotExist<T>(string indexName) where T : ISearchable
        {
            if (!await _searchServiceClient.Indexes.ExistsAsync(indexName))
            {
                await CreateIndex<T>(indexName);
            }
        }

        private async Task CreateIndex<T>(string indexName) where T : ISearchable
        {
            var definition = new Index()
            {
                Name = indexName,
                Fields = FieldBuilder.BuildForType<T>()
            };

            await _searchServiceClient.Indexes.CreateAsync(definition);
        }
    }
}