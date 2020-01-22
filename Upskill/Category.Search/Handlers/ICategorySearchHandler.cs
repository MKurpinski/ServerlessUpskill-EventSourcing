using System.Collections.Generic;
using System.Threading.Tasks;
using Category.Search.Dtos;
using Category.Search.Queries;
using Upskill.Results;
using Upskill.Search.Dtos;

namespace Category.Search.Handlers
{
    public interface ICategorySearchHandler
    {
        Task<PagedSearchResultDto<CategoryDto>> Get(GetCategoriesQuery query);
        Task<IDataResult<CategoryDto>> GetById(GetCategoryByIdQuery query);
        Task<IDataResult<CategoryDto>> GetByName(GetCategoryByNameQuery query);
    }
}
