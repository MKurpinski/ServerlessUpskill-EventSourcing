namespace Application.Search.Queries
{
    public class GetApplicationByIdQuery
    {
        public string Id { get; }

        public GetApplicationByIdQuery(string id)
        {
            Id = id;
        }
    }
}
