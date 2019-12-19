using Application.Storage.Blob.Deleters;
using Application.Storage.Blob.Providers;
using Application.Storage.Blob.Writers;
using Application.Storage.Table.Repository;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Storage.Config;

namespace Application.Storage.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddStorageModule(this IFunctionsHostBuilder builder)
        {
            builder.AddStorage();

            return builder.Services
                .AddTransient<IFileWriter, FileWriter>()
                .AddTransient<IFileDeleter, FileDeleter>()
                .AddTransient<IFileNameProvider, FileNameProvider>()
                .AddTransient<IProcessStatusRepository, ProcessStatusRepository>()
                .AddTransient<ISearchableIndexRepository, SearchableIndexRepository>();
        }
    }
}
