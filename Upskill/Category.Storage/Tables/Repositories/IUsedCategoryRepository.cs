using System.Threading.Tasks;
using Category.Storage.Tables.Models;
using Upskill.Results;

namespace Category.Storage.Tables.Repositories
{
    public interface IUsedCategoryRepository
    {
        Task CreateOrUpdate(string id, int usageCounter);
        Task<IDataResult<IUsedCategory>> GetByCategoryId(string categoryId);
    }
}
