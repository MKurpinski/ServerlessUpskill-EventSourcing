using Application.DataStorage.Options;
using Application.DataStorage.Providers;
using Application.DataStorage.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Infrastructure.Extensions;

namespace Application.DataStorage.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddDataStorageModule(this IFunctionsHostBuilder builder)
        {
            builder.ConfigureOptions<DataStorageOptions>();

            return builder.Services
                .AddTransient<ICosmosClientProvider, CosmosClientProvider>()
                .AddTransient<IDatabaseClientProvider, DatabaseClientProvider>()
                .AddTransient<IContainerClientProvider, ContainerClientProvider>()
                .AddTransient<IApplicationRepository, ApplicationRepository>();
        }
    }
}
