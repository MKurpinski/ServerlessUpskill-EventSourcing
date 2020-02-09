using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Events.Mappers;

namespace Upskill.Events.Extensions
{
    public static class FunctionsHostBuilderExtensions
    {
        public static IServiceCollection AddEventHandler<TEvent, THandler>(this IFunctionsHostBuilder builder) where TEvent : IEvent where THandler : class, IEventHandler<TEvent>
        {
            builder.Services.AddSingleton<ITypeMapper, TypeMapper<TEvent>>();

            return builder.Services
                .AddTransient<IEventHandler<TEvent>, THandler>();
        }

        public static IServiceCollection AddTypeMapping<TEvent>(this IFunctionsHostBuilder builder) where TEvent : IEvent
        {
            return builder.Services.AddSingleton<ITypeMapper, TypeMapper<TEvent>>();
        }
    }
}
