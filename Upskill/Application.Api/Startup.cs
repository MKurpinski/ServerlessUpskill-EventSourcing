using Application.BlobStorage.Config;
using Application.Commands.Config;
using Application.DataStorage.Config;
using Application.Infrastructure.Config;
using Application.RequestMappers.Config;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Application.Api.Startup))]

namespace Application.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.AddRequestMappersModule();
            builder.AddInfrastructureModule();
            builder.AddCommandsModule();
            builder.AddBlobStorageModule();
            builder.AddDataStorageModule();
        }
    }
}
