using System.Threading.Tasks;
using Upskill.Infrastructure.Enums;
using Upskill.LogChecker.Dtos;
using Upskill.Results;

namespace Upskill.LogChecker.Providers
{
    public interface ILastLogProvider
    {
        Task<IDataResult<LogDto>> GetLastLogByCorrelationId(string correlationId);
    }
}
