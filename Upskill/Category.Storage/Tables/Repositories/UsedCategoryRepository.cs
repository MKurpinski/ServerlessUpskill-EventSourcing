using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Category.Storage.Tables.Dtos;
using Category.Storage.Tables.Models;
using Microsoft.WindowsAzure.Storage.Table;
using Upskill.Storage.Table.Providers;
using Upskill.Storage.Table.Repositories;

namespace Category.Storage.Tables.Repositories
{
    public class UsedCategoryRepository : Repository<UsedCategory>, IUsedCategoryRepository
    {
        public UsedCategoryRepository(
            ITableClientProvider tableClientProvider) 
            : base(tableClientProvider)
        {
        }

        public async Task CreateOrUpdate(string id, string usedIn)
        {
            await this.CreateOrUpdate(new UsedCategory(id, usedIn));
        }

        public async Task<CategoryUsageDto> GetCategoryUsageById(string categoryId)
        {
            var queryResult = await this.GetByIdInternal(categoryId);

            return new CategoryUsageDto(categoryId, queryResult.Count);
        }

        private async Task<IList<UsedCategory>> GetByIdInternal(string categoryId)
        {
            var partitionKeyCondition = TableQuery.GenerateFilterCondition(nameof(UsedCategory.PartitionKey), QueryComparisons.Equal, categoryId);

            var query = new TableQuery<UsedCategory>().Where(partitionKeyCondition);

            var queryResult = await this.GetBy(query);
            return queryResult;
        }
    }
}
