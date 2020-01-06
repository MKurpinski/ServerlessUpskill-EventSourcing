using Category.Core.EventHandlers;
using Category.Core.Events;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Events;
using Upskill.Events.Extensions;

namespace Category.Core.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddCoreModule(this IFunctionsHostBuilder builder)
        {
            builder.AddEventHandler<CategoryUsedEvent, CategoryUsedEventHandler>();
            builder.AddEventHandler<CategoryChangedEvent, CategoryChangedEventHandler>();
            return builder.AddEventHandler<CategoryDeletedEvent, CategoryDeletedEventHandler>();
        }
    }
}
