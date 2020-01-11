using System.Net;

namespace Upskill.Storage.Extensions
{
    public static class HttpStatusCodeExtensions
    {
        public static bool IsSuccessfulStatusCode(this int statusCode)
        {
            return statusCode >= (int)HttpStatusCode.OK && statusCode < (int)HttpStatusCode.Ambiguous;
        }
    }
}
