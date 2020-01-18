using Category.EventStore.Facades;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.EventStore.Extensions;

namespace Category.EventStore.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddCategoryEventStore(this IFunctionsHostBuilder builder)
        {
            builder.AddEventStore<Aggregates.Category>();
            return builder.Services.AddTransient<IEventStoreFacade, EventStoreFacade>();
        }
    }
}
