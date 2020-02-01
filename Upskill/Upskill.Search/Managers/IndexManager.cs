using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Upskill.Infrastructure;
using Upskill.Results;
using Upskill.Results.Implementation;
using Upskill.Search.Enums;
using Upskill.Search.Models;
using Upskill.Search.Providers;
using Upskill.Search.Tables.Models;
using Upskill.Search.Tables.Repositories;

namespace Upskill.Search.Managers
{
    public class IndexManager : IIndexManager
    {
        private readonly ISearchableIndexRepository _searchableIndexRepository;
        private readonly ISearchServiceClient _searchServiceClient;
        private readonly IDateTimeProvider _dateTimeProvider;

        public IndexManager(
            ISearchableIndexRepository searchableIndexRepository,
            IDateTimeProvider dateTimeProvider,
            ISearchServiceClientProvider searchServiceClientProvider)
        {
            _searchableIndexRepository = searchableIndexRepository;
            _dateTimeProvider = dateTimeProvider;
            _searchServiceClient = searchServiceClientProvider.Get();
        }

        public async Task<IDataResult<string>> GetIndexNameByType<T>(IndexType indexType) where T : ISearchable
        {
            var indexResult = await this.GetIndexByType<T>(indexType);
            if (!indexResult.Success)
            {
                return new FailedDataResult<string>();
            }
            return new SuccessfulDataResult<string>(indexResult.Value.Name);
        }

        public async Task OpenIndex<T>(IndexType indexType) where T : ISearchable
        {
            var tName = typeof(T).Name;
            var existing = await this.GetIndexNameByType<T>(indexType);

            if (existing.Success)
            {
                return;
            }

            var indexName = $"{tName}-{_dateTimeProvider.GetCurrentDateTime():yyyy-MM-dd-hh-mm-ss}".ToLowerInvariant();
            await _searchableIndexRepository.Create(new SearchableIndex(tName, indexName, indexType.ToString()));
            await this.CreateIndex<T>(indexName);
        }

        public async Task FinishReindexing<T>() where T : ISearchable
        {
            var existingActive = await this.GetIndexByType<T>(IndexType.Active);
            var existingInProgress = await this.GetIndexByType<T>(IndexType.InProgress);

            var toUpdate = new List<SearchableIndex>();

            if (existingActive.Success)
            {
                var newArchived = new SearchableIndex(existingActive.Value.Type, existingActive.Value.Name, IndexType.Archived.ToString());
                toUpdate.Add(newArchived);
            }

            var newActive = new SearchableIndex(existingInProgress.Value.Type, existingInProgress.Value.Name, IndexType.Active.ToString());
            toUpdate.Add(newActive);

            await _searchableIndexRepository.UpdateBatch(toUpdate.ToArray());
        }

        private async Task<IDataResult<SearchableIndex>> GetIndexByType<T>(IndexType indexType)where T : ISearchable
        {
            var tName = typeof(T).Name;
            var index =
                await _searchableIndexRepository.GetByTypeAndStatus(tName, indexType.ToString());
            if (index == null)
            {
                return new FailedDataResult<SearchableIndex>();
            }

            return new SuccessfulDataResult<SearchableIndex>(index);
        }

        private async Task CreateIndex<T>(string indexName) where T : ISearchable
        {
            var definition = new Index
            {
                Name = indexName,
                Fields = FieldBuilder.BuildForType<T>()
            };

            await _searchServiceClient.Indexes.CreateAsync(definition);
        }
    }
}