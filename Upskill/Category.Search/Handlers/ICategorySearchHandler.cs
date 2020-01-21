using System.Collections.Generic;
using System.Threading.Tasks;
using Category.Search.Dtos;
using Category.Search.Queries;
using Upskill.Results;

namespace Category.Search.Handlers
{
    public interface ICategorySearchHandler
    {
        Task<IReadOnlyCollection<CategoryDto>> GetAll();
        Task<IDataResult<CategoryDto>> GetById(GetCategoryByIdQuery query);
        Task<IDataResult<CategoryDto>> GetByName(GetCategoryByNameQuery query);
    }
}
