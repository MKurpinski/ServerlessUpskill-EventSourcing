using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Infrastructure.Extensions;
using Upskill.Search.Managers;
using Upskill.Search.Options;
using Upskill.Search.Providers;
using Upskill.Search.Resolvers;
using Upskill.Search.Tables.Repositories;

namespace Upskill.Search.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddSearch(this IFunctionsHostBuilder builder)
        {
            builder.ConfigureOptions<SearchOptions>();

            return builder.Services
                .AddTransient<ISearchServiceClientProvider, SearchServiceClientProvider>()
                .AddTransient<ISearchIndexClientProvider, SearchIndexClientProvider>()
                .AddTransient<IIndexNameResolver, IndexNameResolver>()
                .AddTransient<IIndexManager, IndexManager>()
                .AddTransient<ISearchableIndexRepository, SearchableIndexRepository>();
        }
    }
}
