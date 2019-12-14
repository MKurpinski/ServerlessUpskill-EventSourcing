namespace Application.Api.CustomHttpRequests
{
    public class SimpleApplicationSearchHttpRequest
    {
        public string Query { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
