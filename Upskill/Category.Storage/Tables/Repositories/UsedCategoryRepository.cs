using System.Threading.Tasks;
using Category.Storage.Tables.Models;
using Upskill.Results;
using Upskill.Results.Implementation;
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

        public async Task CreateOrUpdate(string id, int usageCounter)
        {
            await this.CreateOrUpdate(new UsedCategory(id, usageCounter));
        }

        public async Task<IDataResult<IUsedCategory>> GetByCategoryId(string categoryId)
        {
            var result = await this.GetById(categoryId);

            if (!result.Success)
            {
                return new FailedDataResult<IUsedCategory>();
            }

            return new SuccessfulDataResult<IUsedCategory>(result.Value);
        }
    }
}
