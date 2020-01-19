using Application.Core.EventHandlers;
using Application.Core.Events;
using Application.Core.Events.CreateApplicationProcessStarted;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Events.Extensions;

namespace Application.Core.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddCoreModule(this IFunctionsHostBuilder builder)
        {
            builder.AddEventHandler<CreateApplicationProcessStartedEvent, CreateApplicationProcessStartedEventHandler>();
            builder.AddEventHandler<ApplicationCategoryNameChangedEvent, ApplicationCategoryNameChangedEventHandler>();
            builder.AddEventHandler<ApplicationCreatedEvent, ApplicationCreatedEventHandler>();
            return builder.AddEventHandler<CategoryNameChangedEvent, CategoryNameChangedEventHandler>();
        }
    }
}
