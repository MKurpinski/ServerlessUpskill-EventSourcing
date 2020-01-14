using Application.Category.EventHandlers;
using Application.Category.Events.Incoming;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Events.Extensions;

namespace Application.Category.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddCategories(this IFunctionsHostBuilder builder)
        {
            builder.AddEventHandler<CategoryDeletedEvent, CategoryDeletedEventHandler>();
            builder.AddEventHandler<CategoryCreatedEvent, CategoryCreatedEventHandler>();
            return builder
                .AddEventHandler<CategoryUpdatedEvent, CategoryUpdatedEventHandler>();
        }
    }
}
