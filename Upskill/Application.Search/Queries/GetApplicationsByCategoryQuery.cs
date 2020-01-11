namespace Application.Search.Queries
{
    public class GetApplicationsByCategoryQuery
    {
        public string CategoryName { get; }

        public GetApplicationsByCategoryQuery(
            string categoryName)
        {
            CategoryName = categoryName;
        }
    }
}
