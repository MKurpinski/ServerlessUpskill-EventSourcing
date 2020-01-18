using Microsoft.Azure.WebJobs.Host.Config;

namespace Upskill.RealTimeNotifications.NotificationSubscriberBinding
{
    public class NotificationSubscriberExtensionProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            var provider = new NotificationSubscriberBindingProvider();
            context.AddBindingRule<NotificationSubscriberAttribute>().Bind(provider);
        }
    }
}
