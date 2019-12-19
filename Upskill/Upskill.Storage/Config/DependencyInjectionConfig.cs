using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Infrastructure.Extensions;
using Upskill.Storage.Blob;
using Upskill.Storage.Options;
using Upskill.Storage.Providers;
using Upskill.Storage.Table.Providers;

namespace Upskill.Storage.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddStorage(this IFunctionsHostBuilder builder)
        {
            builder.ConfigureOptions<StorageOptions>();

            return builder.Services
                .AddTransient<IBlobClientProvider, BlobClientProvider>()
                .AddTransient<ITableClientProvider, TableClientProvider>()
                .AddTransient<IStorageAccountProvider, StorageAccountProvider>();
        }
    }
}
