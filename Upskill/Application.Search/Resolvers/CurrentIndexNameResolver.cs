using System.Threading.Tasks;
using Application.Storage.Table.Model;
using Application.Storage.Table.Repository;
using Upskill.Infrastructure;

namespace Application.Search.Resolvers
{
    public class CurrentIndexNameResolver : ICurrentIndexNameResolver
    {
        private readonly ISearchableIndexRepository _searchableIndexRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CurrentIndexNameResolver(
            ISearchableIndexRepository searchableIndexRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _searchableIndexRepository = searchableIndexRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<string> ResolveCurrentIndexName<T>()
        {
            var tName = typeof(T).Name;
            var existingActiveIndex =
                await _searchableIndexRepository.GetByTypeAndStatus(tName, IndexStatus.Active.ToString());

            if (existingActiveIndex != null)
            {
                return existingActiveIndex.Name;
            }

            var indexName = $"{tName}-{_dateTimeProvider.GetCurrentDateTime():yyyy-MM-dd-hh-mm-ss}".ToLowerInvariant();

            await _searchableIndexRepository.Create(new SearchableIndex(tName, indexName, IndexStatus.Active.ToString()));

            return indexName;
        }
    }
}