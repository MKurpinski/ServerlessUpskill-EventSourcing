using Application.Api.Results;
using Application.Commands.Config;
using Application.DataStorage.Config;
using Application.Infrastructure.Config;
using Application.ProcessStatus.Config;
using Application.RequestMappers.Config;
using Application.Storage.Config;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Application.Api.Startup))]

namespace Application.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.AddAppSettingsToConfiguration();
            builder.AddRequestMappersModule();
            builder.AddInfrastructureModule();
            builder.AddCommandsModule();
            builder.AddStorageModule();
            builder.AddDataStorageModule();
            builder.AddProcessStatusModule();
        }
    }
}
