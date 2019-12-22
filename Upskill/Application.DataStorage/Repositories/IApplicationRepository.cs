using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DataStorage.Results;
using Upskill.Results;

namespace Application.DataStorage.Repositories
{
    public interface IApplicationRepository
    {
        Task<IDataResult<Models.Application>> Create(Models.Application application);
        Task<IDataResult<IReadOnlyCollection<Models.Application>>> GetByCategoryName(string categoryName);
        Task<BulkUpdateResult<Models.Application>> BulkUpdateDocuments(IReadOnlyCollection<Models.Application> documentsToUpdate);
    }
}
