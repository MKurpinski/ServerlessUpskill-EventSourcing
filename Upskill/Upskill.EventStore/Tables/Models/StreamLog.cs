using Microsoft.WindowsAzure.Storage.Table;

namespace Upskill.EventStore.Tables.Models
{
    public class StreamLog : TableEntity
    {
        public string Id { get; set; }

        public StreamLog()
        {

        }

        public StreamLog(string id)
        {
            Id = id;
            RowKey = id;
            PartitionKey = id;
        }
    }
}
