using Application.Storage.Blob.Deleters;
using Application.Storage.Blob.Providers;
using Application.Storage.Blob.Writers;
using Application.Storage.Options;
using Application.Storage.Providers;
using Application.Storage.Table.Providers;
using Application.Storage.Table.Repository;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Storage.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddStorageModule(this IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<StorageOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.Bind(settings);
                });

            return builder.Services
                .AddTransient<IFileWriter, FileWriter>()
                .AddTransient<IFileDeleter, FileDeleter>()
                .AddTransient<IFileNameProvider, FileNameProvider>()
                .AddTransient<IBlobClientProvider, BlobClientProvider>()
                .AddTransient<ITableClientProvider, TableClientProvider>()
                .AddTransient<IStorageAccountProvider, StorageAccountProvider>()
                .AddTransient<IProcessStatusRepository, ProcessStatusRepository>();
        }
    }
}
