using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Upskill.EventStore.Builder;

namespace Upskill.EventStore.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddEventStore(this IFunctionsHostBuilder builder)
        { 
            builder.Services.TryAddTransient<IEventStore, EventStore>();
            builder.Services.TryAddTransient<IEventDataBuilder, EventDataBuilder>();
            return builder.Services;
        }
    }
}
