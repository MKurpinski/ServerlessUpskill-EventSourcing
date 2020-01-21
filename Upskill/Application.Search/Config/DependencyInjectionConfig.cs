using Application.Search.Handlers;
using Application.Search.Indexers;
using Application.Search.Managers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Search.Config;

namespace Application.Search.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddSearchModule(this IFunctionsHostBuilder builder)
        {
            builder.AddSearch();

            return builder.Services
                .AddTransient<ISearchableApplicationReindexManager, SearchableApplicationReindexManager>()
                .AddTransient<IApplicationSearchHandler, ApplicationSearchHandler>()
                .AddTransient<ISearchableApplicationIndexer, SearchableApplicationIndexer>();
        }
    }
}
