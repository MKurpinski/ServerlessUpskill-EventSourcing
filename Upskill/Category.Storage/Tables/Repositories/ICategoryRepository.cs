using System.Collections.Generic;
using System.Threading.Tasks;
using Category.Storage.Tables.Dtos;
using Upskill.Results;

namespace Category.Storage.Tables.Repositories
{
    public interface ICategoryRepository
    {
        Task<IDataResult<CategoryDto>> GetById(string id);
        Task<IResult> CreateOrUpdate(CategoryDto category);
        Task<IDataResult<CategoryDto>> GetByName(string name);
        Task<IReadOnlyCollection<CategoryDto>> GetAll();

        Task<IResult> Delete(string id);
    }
}
