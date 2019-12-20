using System;

namespace Application.Storage.Tables.Models
{
    public interface IProcessStatus
    {
        string CorrelationId { get;}
        string Status { get; }
        string Information { get;}
        DateTimeOffset Timestamp { get;}
    }
}
