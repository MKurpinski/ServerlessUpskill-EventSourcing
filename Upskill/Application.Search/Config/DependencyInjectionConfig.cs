using Application.Search.Handlers;
using Application.Search.Indexers;
using Application.Search.Managers;
using Application.Search.Options;
using Application.Search.Providers;
using Application.Search.Resolvers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Infrastructure.Extensions;

namespace Application.Search.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddSearchModule(this IFunctionsHostBuilder builder)
        {
            builder.ConfigureOptions<SearchOptions>();

            return builder.Services
                .AddTransient<ISearchServiceClientProvider, SearchServiceClientProvider>()
                .AddTransient<ISearchIndexClientProvider, SearchIndexClientProvider>()
                .AddTransient<IIndexNameResolver, IndexNameResolver>()
                .AddTransient<IIndexManager, IndexManager>()
                .AddTransient<IApplicationSearchHandler, ApplicationSearchHandler>()
                .AddTransient<ISearchableApplicationIndexer, SearchableApplicationIndexer>();
        }
    }
}
