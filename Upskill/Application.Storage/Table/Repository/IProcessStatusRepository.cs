using System.Threading.Tasks;
using Application.Results;
using Application.Storage.Table.Model;

namespace Application.Storage.Table.Repository
{
    public interface IProcessStatusRepository
    {
        Task CreateOrUpdate(string correlationId, string status, string information);
        Task<IDataResult<IProcessStatus>> GetByCorrelationId(string correlationId);
    }
}
