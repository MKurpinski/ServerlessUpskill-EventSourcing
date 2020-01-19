using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Cache.Config;
using Upskill.RealTimeNotifications.Builders;
using Upskill.RealTimeNotifications.Subscribers;

namespace Upskill.RealTimeNotifications.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddRealTimeNotifications(this IFunctionsHostBuilder builder)
        {
            builder.AddCache();

            return builder.Services
                .AddTransient<INotificationFromEventBuilder, NotificationFromEventBuilder>()
                .AddTransient<ISubscriber, Subscriber>();
        }
    }
}
