using Application.PushNotifications.Factories;
using Application.PushNotifications.Handlers;
using Application.PushNotifications.Options;
using Application.PushNotifications.Providers;
using Application.PushNotifications.Senders;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Infrastructure.Extensions;

namespace Application.PushNotifications.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddPushNotifications(this IFunctionsHostBuilder builder)
        {
            builder.ConfigureOptions<NotificationHubOptions>();

            return builder.Services
                .AddTransient<ISubscriptionHandler, SubscriptionHandler>()
                //.AddTransient<ISender, ApnsSender>()
                .AddTransient<ISender, FcmSender>()
                .AddTransient<IPushNotificationSender, PushNotificationSender>()
                .AddTransient<IHubConnectionProvider, HubConnectionProvider>()
                .AddTransient<IRegistrationDescriptionFactory, RegistrationDescriptionFactory>();
        }
    }
}
