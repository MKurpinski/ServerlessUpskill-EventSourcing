using System.Threading.Tasks;
using Application.Results;

namespace Application.DataStorage.Repositories
{
    public interface IApplicationRepository
    {
        Task<IDataResult<Model.Application>> Create(Model.Application application);
    }
}
