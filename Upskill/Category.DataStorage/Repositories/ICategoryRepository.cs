using System.Collections.Generic;
using System.Threading.Tasks;
using Category.DataStorage.Dtos;
using Upskill.Results;

namespace Category.DataStorage.Repositories
{
    public interface ICategoryRepository
    {
        Task<IDataResult<CategoryDto>> GetById(string id);
        Task<IDataResult<CategoryDto>> GetByName(string name);
        Task<IReadOnlyCollection<CategoryDto>> GetAll();
        Task<IResult> Create(CategoryDto categoryDto);
        Task<IResult> Update(CategoryDto categoryDto);
        Task<IResult> Delete(string id);
    }
}
