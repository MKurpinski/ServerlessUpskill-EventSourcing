namespace Category.Search.Queries
{
    public class GetCategoriesQuery
    {
        public int Skip { get; }
        public int Take { get; }

        public GetCategoriesQuery(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }
    }
}
