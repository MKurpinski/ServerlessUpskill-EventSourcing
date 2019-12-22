namespace Category.Storage.Tables.Dtos
{
    public class CategoryUsageDto
    {
        public string Id { get; }
        public int UsageCounter { get; }

        public CategoryUsageDto(string id, int usageCounter)
        {
            Id = id;
            UsageCounter = usageCounter;
        }
    }
}
