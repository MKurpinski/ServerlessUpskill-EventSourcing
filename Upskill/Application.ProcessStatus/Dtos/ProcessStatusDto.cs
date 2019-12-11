using System;

namespace Application.ProcessStatus.Dtos
{
    public class ProcessStatusDto
    {
        public DateTime LastUpdated { get; }
        public string Status { get; }

        public ProcessStatusDto(DateTime lastUpdated, string status)
        {
            LastUpdated = lastUpdated;
            Status = status;
        }
    }
}
