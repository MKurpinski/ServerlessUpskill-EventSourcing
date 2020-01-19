using Microsoft.Azure.WebJobs;

namespace Upskill.RealTimeNotifications.NotificationSubscriberBinding
{
    public static class NotificationSubscriberExtensions
    {
        public static IWebJobsBuilder AddNotificationSubscriberBinding(this IWebJobsBuilder builder)
        {
            builder.AddExtension<NotificationSubscriberExtensionProvider>();
            return builder;
        }
    }
}
