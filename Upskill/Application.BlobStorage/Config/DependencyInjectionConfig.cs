using Application.BlobStorage.Options;
using Application.BlobStorage.Providers;
using Application.BlobStorage.Writers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.BlobStorage.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddBlobStorageModule(this IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<BlobStorageOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.Bind(settings);
                });

            return builder.Services
                .AddTransient<IFileWriter, FileWriter>()
                .AddTransient<IBlobClientProvider, BlobClientProvider>();
        }
    }
}
