using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Search.Dtos;
using Application.Search.Queries;
using Upskill.Results;
using Upskill.Search.Dtos;

namespace Application.Search.Handlers
{
    public interface IApplicationSearchHandler
    {
        Task<PagedSearchResultDto<SimpleApplicationDto>> Search(SimpleApplicationSearchQuery query);
        Task<IDataResult<ApplicationDto>> GetById(GetApplicationByIdQuery query);
        Task<IEnumerable<ApplicationDto>> GetByCategory(GetApplicationsByCategoryQuery query);
    }
}
