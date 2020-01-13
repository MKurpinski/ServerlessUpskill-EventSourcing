using Microsoft.WindowsAzure.Storage.Table;

namespace Application.Storage.Tables.Models
{
    public class ApplicationCategory : TableEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public ApplicationCategory()
        {
        }

        public ApplicationCategory(string id, string name)
        {
            Id = id;
            Name = name;
            PartitionKey = nameof(ApplicationCategory);
            RowKey = Id;
        }
    }
}
