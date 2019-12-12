using Application.Storage.Blob.Deleters;
using Application.Storage.Blob.Providers;
using Application.Storage.Blob.Writers;
using Application.Storage.Options;
using Application.Storage.Providers;
using Application.Storage.Table.Providers;
using Application.Storage.Table.Repository;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Infrastructure.Extensions;

namespace Application.Storage.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddStorageModule(this IFunctionsHostBuilder builder)
        {
            builder.ConfigureOptions<StorageOptions>();

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
