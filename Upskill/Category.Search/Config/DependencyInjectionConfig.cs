using Category.Search.Handlers;
using Category.Search.Indexers;
using Category.Search.Managers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Search.Config;

namespace Category.Search.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddSearchModule(this IFunctionsHostBuilder builder)
        {
            builder.AddSearch();

            return builder.Services
                .AddTransient<ISearchableCategoryReindexManager, SearchableCategoryReindexManager>()
                .AddTransient<ICategorySearchHandler, CategorySearchHandler>()
                .AddTransient<ISearchableCategoryIndexer, SearchableCategoryIndexer>();
        }
    }
}
