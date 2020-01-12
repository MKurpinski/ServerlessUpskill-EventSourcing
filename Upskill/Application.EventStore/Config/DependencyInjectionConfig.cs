using Application.EventStore.Facades;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.EventStore.Config;

namespace Application.EventStore.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddApplicationEventStore(this IFunctionsHostBuilder builder)
        {
            builder.AddEventStore();
            return builder.Services.AddTransient<IEventStoreFacade, EventStoreFacade>();
        }
    }
}
