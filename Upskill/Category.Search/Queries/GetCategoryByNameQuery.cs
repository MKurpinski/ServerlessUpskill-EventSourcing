namespace Category.Search.Queries
{
    public class GetCategoryByNameQuery
    {
        public string Name { get; }

        public GetCategoryByNameQuery(string name)
        {
            Name = name;
        }
    }
}
