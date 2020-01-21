namespace Category.Search.Queries
{
    public class GetCategoryByIdQuery
    {
        public string Id { get; }

        public GetCategoryByIdQuery(string id)
        {
            Id = id;
        }
    }
}
