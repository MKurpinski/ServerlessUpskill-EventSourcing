using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.EventPublisher.Client;
using Upskill.EventPublisher.Options;
using Upskill.EventPublisher.Publishers;
using Upskill.Infrastructure.Extensions;

namespace Upskill.EventPublisher.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddEventPublisher(this IFunctionsHostBuilder builder)
        {
            builder.ConfigureOptions<EventOptions>();

            return builder.Services
                .AddTransient<IEventPublisher, Publishers.EventPublisher>()
                .AddTransient<IEventGridClientFacade, EventGridClientFacade>();
        }
    }
}
