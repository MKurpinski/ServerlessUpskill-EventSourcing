using Category.Api;
using Category.Api.CustomHttpRequests;
using Category.Api.Validators;
using Category.Storage.Config;
using FluentValidation;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.EventPublisher.Config;
using Upskill.FunctionUtils.Extensions;
using Upskill.Infrastructure.Config;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Category.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IValidator<IModifyCategoryHttpRequest>, ModifyCategoryHttpRequestValidator>();

            builder.AddAppSettingsToConfiguration();
            builder.AddStorageModule();
            builder.AddInfrastructureModule();
            builder.AddEventPublisher();
        }
    }
}
