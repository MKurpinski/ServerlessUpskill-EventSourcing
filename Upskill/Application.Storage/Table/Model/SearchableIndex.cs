using Microsoft.WindowsAzure.Storage.Table;

namespace Application.Storage.Table.Model
{
    public class SearchableIndex : TableEntity
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public SearchableIndex(string type, string name, string status)
        {
            Type = type;
            Name = name;
            Status = status;
            PartitionKey = Type;
            RowKey = Name;
        }

        public SearchableIndex()
        {

        }
    }
}
