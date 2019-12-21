using System.Threading.Tasks;
using Application.Storage.Dtos;
using Upskill.Results;

namespace Application.Storage.Tables.Repositories
{
    public interface ICategoryRepository
    {
        Task<IDataResult<CategoryDto>> GetById(string id);
        Task CreateOrUpdate(CategoryDto category);
        Task Delete(string id);
    }
}
