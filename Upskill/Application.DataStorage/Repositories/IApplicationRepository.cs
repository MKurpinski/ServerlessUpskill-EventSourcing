using System.Threading.Tasks;
using Upskill.Results;

namespace Application.DataStorage.Repositories
{
    public interface IApplicationRepository
    {
        Task<IDataResult<Models.Application>> Create(Models.Application application);
    }
}
