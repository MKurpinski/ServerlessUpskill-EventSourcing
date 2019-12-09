using System;

namespace Application.Storage.Table.Model
{
    public interface IProcessStatus
    {
        string CorrelationId { get;}
        string Status { get; }
        string Information { get;}
        DateTimeOffset Timestamp { get;}
    }
}
