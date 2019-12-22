using Application.Category.Handlers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Category.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddCategories(this IFunctionsHostBuilder builder)
        {
            return builder.Services
                .AddTransient<ICategoryNameChangedHandler, CategoryNameChangedHandler>();
        }
    }
}
