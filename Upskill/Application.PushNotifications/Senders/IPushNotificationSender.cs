using System.Threading.Tasks;

namespace Application.PushNotifications.Senders
{
    public interface IPushNotificationSender
    {
        Task SendNotification<T>(T notification, string message, string[] tags = null);
    }
}
