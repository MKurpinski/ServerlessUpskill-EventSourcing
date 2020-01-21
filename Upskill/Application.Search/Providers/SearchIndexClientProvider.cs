﻿using System.Threading.Tasks;
using Application.Search.Enums;
using Application.Search.Models;
using Application.Search.Resolvers;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace Application.Search.Providers
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