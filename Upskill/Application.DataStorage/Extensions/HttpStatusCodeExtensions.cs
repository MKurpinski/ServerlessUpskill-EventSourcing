using System.Net;

namespace Application.DataStorage.Extensions
{
    public static class HttpStatusCodeExtensions
    {
        public static bool IsSuccessfulStatusCode(this HttpStatusCode statusCode)
        {
            return statusCode >= HttpStatusCode.OK && statusCode < HttpStatusCode.Ambiguous;
        }
    }
}
