namespace Application.Search.Queries
{
    public class SimpleApplicationSearchQuery : IQuery
    {
        public string SearchPhrase { get;}
        public int Skip { get; }
        public int Take { get; }

        public SimpleApplicationSearchQuery(string searchPhrase, int skip, int take)
        {
            SearchPhrase = searchPhrase;
            Skip = skip;
            Take = take;
        }
    }
}
