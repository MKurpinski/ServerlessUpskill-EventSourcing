using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.EventsInfrastructure.Clients;
using Upskill.EventsInfrastructure.Dispatchers;
using Upskill.EventsInfrastructure.Options;
using Upskill.EventsInfrastructure.Providers;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.Infrastructure.Extensions;

namespace Upskill.EventsInfrastructure.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddEvents(this IFunctionsHostBuilder builder)
        {
            builder.ConfigureOptions<EventOptions>();

            return builder.Services
                .AddTransient<IEventPublisher, EventPublisher>()
                .AddTransient<IEventDispatcher, EventDispatcher>()
                .AddTransient<IHandlerImplementationProvider, HandlerImplementationProvider>()
                .AddSingleton<IEventTypeProvider, EventTypeProvider>()
                .AddTransient<IEventGridClientFacade, EventGridClientFacade>();
        }
    }
}
