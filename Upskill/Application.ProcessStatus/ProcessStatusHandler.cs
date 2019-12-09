using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.ProcessStatus.Dtos;
using Application.ProcessStatus.Enums;
using Application.Results;
using Application.Results.Implementation;
using Application.Storage.Table.Repository;

namespace Application.ProcessStatus
{
    public class ProcessStatusHandler : IProcessStatusHandler
    {
        private readonly IProcessStatusRepository _processStatusRepository;

        public ProcessStatusHandler(IProcessStatusRepository processStatusRepository)
        {
            _processStatusRepository = processStatusRepository;
        }
        public async Task TrackStatus(string correlationId, string status, IEnumerable<KeyValuePair<string, string>> errors)
        {
            await _processStatusRepository.CreateOrUpdate(
                correlationId,
                status,
                string.Join(",", errors.Select(err => $"{err.Key}: {err.Value}")));
        }

        public async Task<IDataResult<ProcessStatusDto>> GetStatus(string correlationId)
        {
            var statusResult = await _processStatusRepository.GetByCorrelationId(correlationId);

            if (!statusResult.Success)
            {
                return new FailedDataResult<ProcessStatusDto>();
            }

            return new SuccessfulDataResult<ProcessStatusDto>(new ProcessStatusDto(statusResult.Value.Timestamp.DateTime, statusResult.Value.Status));
        }
    }
}
