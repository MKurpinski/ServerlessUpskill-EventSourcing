namespace Category.Search.Dtos
{
    public class CategoryDto
    {
        public string Id { get; }
        public string Name { get; }
        public string Description { get; }
        public int SortOrder { get; }

        public CategoryDto(
            string id,
            string name,
            string description,
            int sortOrder)
        {
            Id = id;
            Name = name;
            Description = description;
            SortOrder = sortOrder;
        }
    }
}
