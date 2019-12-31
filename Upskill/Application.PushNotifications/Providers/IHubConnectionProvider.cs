using Microsoft.Azure.NotificationHubs;

namespace Application.PushNotifications.Providers
{
    public interface IHubConnectionProvider
    {
        NotificationHubClient Get();
    }
}
