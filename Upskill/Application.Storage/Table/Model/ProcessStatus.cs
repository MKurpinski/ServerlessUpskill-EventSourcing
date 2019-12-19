using Microsoft.WindowsAzure.Storage.Table;

namespace Application.Storage.Table.Model
{
    public class ProcessStatus : TableEntity, IProcessStatus
    {
        public string CorrelationId { get; set; }
        public string Status { get; set; }
        public string Information { get; set; }

        public ProcessStatus()
        {

        }

        public ProcessStatus(string correlationId, string status, string information)
        {
            CorrelationId = correlationId;
            Status = status;
            Information = information;
            RowKey = correlationId;
            PartitionKey = nameof(ProcessStatus);
        }
    }
}
