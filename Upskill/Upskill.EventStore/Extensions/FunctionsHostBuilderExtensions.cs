using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Upskill.EventStore.Appliers;
using Upskill.EventStore.Builder;
using Upskill.EventStore.Models;
using Upskill.EventStore.Providers;
using Upskill.EventStore.Tables.Repositories;

namespace Upskill.EventStore.Extensions
{
    public static class FunctionsHostBuilderExtensions
    {
        public static IServiceCollection AddEventStore<T>(this IFunctionsHostBuilder builder) where T : IAggregateRoot
        {
            builder.Services.TryAddTransient<IEventDataBuilder, EventDataBuilder>();
            builder.Services.TryAddTransient<IEventsApplier, EventsApplier>();
            builder.Services
                .AddTransient<IStreamProvider<T>, StreamProvider<T>>();
            builder.Services
                .AddTransient<IStreamLogRepository<T>, StreamLogRepository<T>>();
            builder.Services
                .AddTransient<IStreamLogProvider<T>, StreamLogProvider<T>>();

            return builder.Services
                .AddTransient<IEventStore<T>, EventStore<T>>();
        }
    }
}
