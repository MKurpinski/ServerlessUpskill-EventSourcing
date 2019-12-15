using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly ILogger _logger;

        public BaseIndexer(ISearchIndexClientProvider searchIndexClientProvider, ILogger logger)
        {
            _searchIndexClientProvider = searchIndexClientProvider;
            _logger = logger;
        }

        protected async Task Index(T value)
        {
            var indexClient = await _searchIndexClientProvider.Get<T>();

            var batch = IndexBatch.MergeOrUpload<T>(new List<T> { value });

            try
            {
                await indexClient.Documents.IndexAsync<T>(batch);
            }
            catch (IndexBatchException)
            {
                // ToDo Ask
                _logger.LogError($"Indexing failed for ${value.Id} in index {indexClient.IndexName}");
            }
        }
    }
}
