using System.Collections.Generic;
using System.Threading.Tasks;
using Application.ProcessStatus.Dtos;
using Application.Results;

namespace Application.ProcessStatus
{
    public interface IProcessStatusHandler
    {
        Task TrackStatus(string correlationId, string status, IEnumerable<KeyValuePair<string, string>> errors);
        Task<IDataResult<ProcessStatusDto>> GetStatus(string correlationId);
    }
}
