namespace Application.Search.Options
{
    public class SearchOptions
    {
        public string SearchServiceName { get; set; }
        public string SearchServiceAdminKey { get; set; }
        public int SharedAccessSignatureLifetimeInHours { get; set; }
    }
}