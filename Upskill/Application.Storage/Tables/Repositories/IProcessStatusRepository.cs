using System.Threading.Tasks;
using Application.Storage.Tables.Models;
using Upskill.Results;

namespace Application.Storage.Tables.Repositories
{
    public interface IProcessStatusRepository
    {
        Task CreateOrUpdate(string correlationId, string status, string information);
        Task<IDataResult<IProcessStatus>> GetByCorrelationId(string correlationId);
    }
}
