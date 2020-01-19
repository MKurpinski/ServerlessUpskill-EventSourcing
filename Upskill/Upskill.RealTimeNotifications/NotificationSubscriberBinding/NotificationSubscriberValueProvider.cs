using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Upskill.RealTimeNotifications.Constants;

namespace Upskill.RealTimeNotifications.NotificationSubscriberBinding
{
    public class NotificationSubscriberValueProvider : IValueProvider
    {
        private readonly HttpRequest _request;

        public NotificationSubscriberValueProvider(HttpRequest request)
        {
            _request = request;
        }

        public Task<object> GetValueAsync()
        {
            if (!_request.Headers.ContainsKey(NotificationUserId.Name))
            {
                return Task.FromResult<object>(string.Empty);
            }

            var subscriberHeader = _request.Headers[NotificationUserId.Name].ToString();
            return Task.FromResult<object>(subscriberHeader);
        }

        public Type Type => typeof(string);

        public string ToInvokeString() => string.Empty;
    }
}
