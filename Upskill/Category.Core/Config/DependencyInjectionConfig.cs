using Category.Core.EventHandlers;
using Category.Core.Events.External;
using Category.Core.Events.Internal;
using Category.Core.Validators;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Events.Extensions;

namespace Category.Core.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddCoreModule(this IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IDeleteValidator, DeleteValidator>();

            builder.AddEventHandler<CategoryUsedEvent, CategoryUsedEventHandler>();
            builder.AddEventHandler<UpdateCategoryProcessStartedEvent, UpdateCategoryProcessStartedEventHandler>();
            builder.AddEventHandler<CreateCategoryProcessStartedEvent, CreateCategoryProcessStartedEventHandler>();
            return builder.AddEventHandler<DeleteCategoryProcessStartedEvent, DeleteCategoryProcessStartedEventHandler>();
        }
    }
}
