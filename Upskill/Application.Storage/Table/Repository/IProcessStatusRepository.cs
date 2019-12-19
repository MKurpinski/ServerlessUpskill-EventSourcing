using System.Threading.Tasks;
using Application.Storage.Table.Model;
using Upskill.Results;

namespace Application.Storage.Table.Repository
{
    public interface IProcessStatusRepository
    {
        Task CreateOrUpdate(string correlationId, string status, string information);
        Task<IDataResult<IProcessStatus>> GetByCorrelationId(string correlationId);
    }
}
