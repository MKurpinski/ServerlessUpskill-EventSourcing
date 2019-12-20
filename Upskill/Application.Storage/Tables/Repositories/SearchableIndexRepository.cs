using System.Linq;
using System.Threading.Tasks;
using Application.Storage.Tables.Models;
using Microsoft.WindowsAzure.Storage.Table;
using Upskill.Storage.Table.Providers;
using Upskill.Storage.Table.Repositories;

namespace Application.Storage.Tables.Repositories
{
    public class SearchableIndexRepository : Repository<SearchableIndex>, ISearchableIndexRepository
    {
        public SearchableIndexRepository(ITableClientProvider tableClientProvider) : base(tableClientProvider)
        {
        }

        public Task Create(SearchableIndex searchableIndex)
        {
            return this.CreateOrUpdate(searchableIndex);
        }

        public async Task<SearchableIndex> GetByTypeAndStatus(string type, string status)
        {
            var typeCondition = TableQuery.GenerateFilterCondition(nameof(SearchableIndex.PartitionKey), QueryComparisons.Equal, type);
            var statusCondition = TableQuery.GenerateFilterCondition(nameof(SearchableIndex.Status), QueryComparisons.Equal, status);

            var combinedFilter = TableQuery.CombineFilters(typeCondition, TableOperators.And, statusCondition);
            var query = new TableQuery<SearchableIndex>().Where(combinedFilter);

            var queryResult = await this.GetBy(query);

            return queryResult.FirstOrDefault();
        }
    }
}