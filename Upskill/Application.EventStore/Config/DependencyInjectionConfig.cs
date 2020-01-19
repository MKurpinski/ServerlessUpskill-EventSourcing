using Application.EventStore.Facades;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.EventStore.Extensions;

namespace Application.EventStore.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddApplicationEventStore(this IFunctionsHostBuilder builder)
        {
            builder.AddEventStore<Aggregates.Application>();
            return builder.Services.AddTransient<IEventStoreFacade, EventStoreFacade>();
        }
    }
}
