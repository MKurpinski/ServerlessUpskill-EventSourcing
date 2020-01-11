using Application.Core.EventHandlers;
using Application.Core.Events;
using Application.Core.Events.ApplicationAddedEvent;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Events.Extensions;

namespace Application.Core.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddCoreModule(this IFunctionsHostBuilder builder)
        {
            builder.AddEventHandler<ApplicationAddedEvent, ApplicationAddedEventHandler>();
            builder.AddEventHandler<ApplicationCategoryNameChangedEvent, ApplicationCategoryNameChangedEventHandler>();
            return builder.AddEventHandler<CategoryNameChangedEvent, CategoryNameChangedEventHandler>();
        }
    }
}
