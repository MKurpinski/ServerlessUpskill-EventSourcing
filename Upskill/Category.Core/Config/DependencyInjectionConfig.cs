using Category.Core.EventHandlers;
using Category.Core.Events;
using Category.Core.Guards;
using Category.Core.Validators;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Events.Extensions;
using Upskill.EventStore.Extensions;

namespace Category.Core.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddCoreModule(this IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<ICategoryQueueingEventGuard, CategoryQueueingEventGuard>();
            builder.Services.AddTransient<IDeleteValidator, DeleteValidator>();
            builder.AddEventStore<Aggregates.Category>();
            builder.AddEventHandler<CategoryUsedEvent, CategoryUsedEventHandler>();
            builder.AddEventHandler<UpdateCategoryProcessStartedEvent, UpdateCategoryProcessStartedEventHandler>();
            builder.AddEventHandler<CreateCategoryProcessStartedEvent, CreateCategoryProcessStartedEventHandler>();
            
            builder.AddTypeMapping<CategoryCreatedEvent>();
            builder.AddTypeMapping<CategoryUpdatedEvent>();
            builder.AddTypeMapping<CategoryDeletedEvent>();

            return builder.AddEventHandler<DeleteCategoryProcessStartedEvent, DeleteCategoryProcessStartedEventHandler>();
        }
    }
}
