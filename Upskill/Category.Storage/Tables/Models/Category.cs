using Microsoft.WindowsAzure.Storage.Table;

namespace Category.Storage.Tables.Models
{
    public class Category : TableEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }

        public Category()
        {

        }

        public Category(string id, string name, string description, int sortOrder)
        {
            Id = id;
            Name = name;
            Description = description;
            SortOrder = sortOrder;
            PartitionKey = nameof(Category);
            RowKey = Id;
        }
    }
}
