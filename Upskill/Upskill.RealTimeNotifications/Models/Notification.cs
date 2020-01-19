namespace Upskill.RealTimeNotifications.Models
{
    public class Notification
    {
        public string To { get; }
        public NotificationContent Content { get; }

        public Notification(
            string to,
            NotificationContent content)
        {
            To = to;
            Content = content;
        }
    }
}
