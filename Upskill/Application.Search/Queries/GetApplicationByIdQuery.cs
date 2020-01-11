namespace Application.Search.Queries
{
    public class GetApplicationByIdQuery: IQuery
    {
        public string Id { get; }

        public GetApplicationByIdQuery(string id)
        {
            Id = id;
        }
    }
}
