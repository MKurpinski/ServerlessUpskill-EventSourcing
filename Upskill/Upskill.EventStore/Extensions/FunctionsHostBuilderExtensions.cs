using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Upskill.EventStore.Builder;
using Upskill.EventStore.Models;

namespace Upskill.EventStore.Extensions
{
    public static class FunctionsHostBuilderExtensions
    {
        public static IServiceCollection AddEventStore<T>(this IFunctionsHostBuilder builder) where T : IAggregate
        {
            builder.Services.TryAddTransient<IEventDataBuilder, EventDataBuilder>();

            return builder.Services
                .AddTransient<IEventStore<T>, EventStore<T>>();
        }
    }
}
