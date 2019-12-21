using Microsoft.WindowsAzure.Storage.Table;

namespace Application.Storage.Tables.Models
{
    public class Category : TableEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public Category()
        {
        }

        public Category(string id, string name)
        {
            Id = id;
            Name = name;
            PartitionKey = nameof(Category);
            RowKey = Id;
        }
    }
}
