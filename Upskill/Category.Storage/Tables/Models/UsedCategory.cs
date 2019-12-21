using Microsoft.WindowsAzure.Storage.Table;

namespace Category.Storage.Tables.Models
{
    public class UsedCategory : TableEntity, IUsedCategory
    {
        public string Id { get; set; }
        public string UsedIn { get; }

        public UsedCategory()
        {

        }

        public UsedCategory(string id, string usedIn)
        {
            PartitionKey = id;
            Id = id;
            RowKey = usedIn;
            UsedIn = usedIn;
        }
    }
}
