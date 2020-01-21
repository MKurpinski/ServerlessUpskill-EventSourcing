using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Search.Enums;
using Application.Search.Models;
using Application.Search.Providers;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Logging;

namespace Application.Search.Indexers
{
    public class BaseIndexer<T> where T : ISearchable
    {
        private readonly ISearchIndexClientProvider _searchIndexClientProvider;
        private readonly ILogger<BaseIndexer<T>> _logger;

        public BaseIndexer(ISearchIndexClientProvider searchIndexClientProvider, ILogger<BaseIndexer<T>> logger)
        {
            _searchIndexClientProvider = searchIndexClientProvider;
            _logger = logger;
        }

        protected async Task Index(T value, IndexType indexType)
        {
            var indexClient = await _searchIndexClientProvider.Get<T>(indexType);

            var batch = IndexBatch.MergeOrUpload<T>(new List<T> { value });

            try
            {
                await indexClient.Documents.IndexAsync<T>(batch);
            }
            catch (IndexBatchException)
            {
                _logger.LogError($"Indexing failed for ${value.Id} in index {indexClient.IndexName}");
            }
        }

        protected async Task OpenNewIndex()
        {
            await _searchIndexClientProvider.Get<T>(IndexType.InProgress);
        }
    }
}
