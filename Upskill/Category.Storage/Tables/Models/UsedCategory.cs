using Microsoft.WindowsAzure.Storage.Table;

namespace Category.Storage.Tables.Models
{
    public class UsedCategory : TableEntity, IUsedCategory
    {
        public string Id { get; set; }
        public int UsageCounter { get; set; }

        public UsedCategory()
        {

        }

        public UsedCategory(string id, int usageCounter)
        {
            PartitionKey = nameof(UsedCategory);
            Id = id;
            RowKey = Id;
            UsageCounter = usageCounter;
        }
    }
}
