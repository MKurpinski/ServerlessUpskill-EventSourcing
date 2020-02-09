using Newtonsoft.Json;

namespace Upskill.LogChecker.Clients.Models
{
    public class LogAnalyticsRequest
    {
        [JsonProperty("query")]
        public string Query { get; }

        public LogAnalyticsRequest(string query)
        {
            Query = query;
        }
    }
}
