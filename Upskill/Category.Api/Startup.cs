using Category.Api;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Upskill.FunctionUtils.Extensions;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Category.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.AddAppSettingsToConfiguration();
        }
    }
}
