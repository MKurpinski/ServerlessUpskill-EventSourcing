using System.Threading.Tasks;
using RestEase;
using Upskill.LogChecker.Clients.Models;

namespace Upskill.LogChecker.Clients
{
    public interface ILogAnalyticsClient
    {
        [Path("applicationId")]
        string ApplicationId { get; set; }

        [Header("x-api-key")]
        string ApiKey { get; set; }


        [Post("/apps/{applicationId}/query")]
        Task<Response<LogAnalyticsResponse>> GetLogs([Body] LogAnalyticsRequest query);
    }
}
