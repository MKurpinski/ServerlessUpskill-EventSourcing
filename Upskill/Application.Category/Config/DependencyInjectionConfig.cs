using Application.Category.EventHandlers;
using Application.Category.Events.Incoming;
using Application.Core.EventHandlers;
using Application.Core.Events.ApplicationChangedEvent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Events;
using Upskill.Events.Extensions;
using Upskill.EventsInfrastructure.Extensions;

namespace Application.Category.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddCategories(this IFunctionsHostBuilder builder)
        {
            builder
                .AddEventHandler<CategoryDeletedEvent, CategoryDeletedEventHandler>();

            return builder
                .AddEventHandler<CategoryChangedEvent, CategoryChangedEventHandler>();
        }
    }
}
