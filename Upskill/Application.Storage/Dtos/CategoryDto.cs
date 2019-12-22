namespace Application.Storage.Dtos
{
    public class CategoryDto
    {
        public string Id { get; }
        public string Name { get; }

        public CategoryDto(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
