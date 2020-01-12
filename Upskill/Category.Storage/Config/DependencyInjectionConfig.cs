using Category.Storage.Tables.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Storage.Config;

namespace Category.Storage.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddStorageModule(this IFunctionsHostBuilder builder)
        {
            builder.AddStorage();
            builder.Services
                .AddTransient<ICategoryRepository, CategoryRepository>();
            return builder.Services
                .AddTransient<IUsedCategoryRepository, UsedCategoryRepository>();
        }
    }
}
