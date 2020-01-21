using System.Threading.Tasks;
using Upskill.Infrastructure;
using Upskill.Results;
using Upskill.Results.Implementation;
using Upskill.Search.Enums;
using Upskill.Search.Models;
using Upskill.Search.Tables.Models;
using Upskill.Search.Tables.Repositories;

namespace Upskill.Search.Managers
{
    public abstract class IndexManager : IIndexManager
    {
        private readonly ISearchableIndexRepository _searchableIndexRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        protected IndexManager(
            ISearchableIndexRepository searchableIndexRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _searchableIndexRepository = searchableIndexRepository;
            _dateTimeProvider = dateTimeProvider;
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

        public async Task StartReindex<T>() where T : ISearchable
        {
            await this.OpenIndex<T>(IndexType.InProgress);
        }

        public async Task BuildIndex<T>() where T : ISearchable
        {
            await this.OpenIndex<T>(IndexType.Active);
        }

        private async Task OpenIndex<T>(IndexType indexType) where T : ISearchable
        {
            var tName = typeof(T).Name;
            var existing = await this.GetIndexNameByType<T>(indexType);

            if (existing.Success)
            {
                return;
            }

            var indexName = $"{tName}-{_dateTimeProvider.GetCurrentDateTime():yyyy-MM-dd-hh-mm-ss}".ToLowerInvariant();
            await _searchableIndexRepository.Create(new SearchableIndex(tName, indexName, indexType.ToString()));
        }

        public async Task FinishReindexing<T>() where T : ISearchable
        {
            var existingActive = await this.GetIndexByType<T>(IndexType.Active);
            var existingInProgress = await this.GetIndexByType<T>(IndexType.InProgress);

            var newArchived = new SearchableIndex(existingActive.Value.Type, existingActive.Value.Name, IndexType.Archived.ToString());
            var newActive = new SearchableIndex(existingInProgress.Value.Type, existingInProgress.Value.Name, IndexType.Active.ToString());

            await _searchableIndexRepository.UpdateBatch(newActive, newArchived);
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
    }
}