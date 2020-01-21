using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Logging;
using Upskill.Results;
using Upskill.Results.Implementation;
using Upskill.Search.Enums;
using Upskill.Search.Models;
using Upskill.Search.Providers;

namespace Upskill.Search.Indexers
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

        protected async Task<IResult> DeleteInternal(string id)
        {
            var indexClient = await _searchIndexClientProvider.Get<T>(IndexType.Active);

            var batch = IndexBatch.Delete(new List<string> { id });

            try
            {
                await indexClient.Documents.IndexAsync<string>(batch);
                return new SuccessfulResult();
            }
            catch (IndexBatchException)
            {
                _logger.LogError($"Removing failed for ${id} in index {indexClient.IndexName}");
                return new FailedResult();
            }
        }

        protected async Task OpenNewIndex()
        {
            await _searchIndexClientProvider.Get<T>(IndexType.InProgress);
        }
    }
}
