using System.Threading.Tasks;
using Category.Storage.Tables.Dtos;

namespace Category.Storage.Tables.Repositories
{
    public interface IUsedCategoryRepository
    {
        Task CreateOrUpdate(string id, string usedIn);
        Task<CategoryUsageDto> GetCategoryUsageById(string categoryId);
    }
}
