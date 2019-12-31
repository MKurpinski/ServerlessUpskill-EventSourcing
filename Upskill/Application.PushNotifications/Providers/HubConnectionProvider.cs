using Application.PushNotifications.Options;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Options;

namespace Application.PushNotifications.Providers
{
    public class HubConnectionProvider : IHubConnectionProvider
    {
        private readonly NotificationHubOptions _options;

        public HubConnectionProvider(IOptions<NotificationHubOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public NotificationHubClient Get()
        {
            return new NotificationHubClient(_options.ConnectionString, _options.HubName);
        }
    }
}