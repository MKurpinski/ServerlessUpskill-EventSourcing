using Application.Storage.Blobs.Deleters;
using Application.Storage.Blobs.Providers;
using Application.Storage.Blobs.Writers;
using Application.Storage.Tables.Repositories;
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
                .AddTransient<ISharedAccessSignatureProvider, SharedAccessSignatureProvider>()
                .AddTransient<IFileNameProvider, FileNameProvider>()
                .AddTransient<IProcessStatusRepository, ProcessStatusRepository>()
                .AddTransient<ICategoryRepository, CategoryRepository>()
                .AddTransient<ISearchableIndexRepository, SearchableIndexRepository>();
        }
    }
}
