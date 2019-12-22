using Category.Api.Commands;
using Category.Api.CustomHttpRequests;
using Category.Api.Validators;
using Category.DataStorage.Config;
using Category.Storage.Config;
using FluentValidation;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.EventPublisher.Config;
using Upskill.FunctionUtils.Extensions;
using Upskill.Infrastructure.Config;

[assembly: FunctionsStartup(typeof(Category.Api.Startup))]

namespace Category.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IValidator<CreateCategoryHttpRequest>, CreateCategoryHttpRequestValidator>();
            builder.Services.AddTransient<IValidator<UpdateCategoryCommand>, UpdateCategoryCommandValidator>();

            builder.AddAppSettingsToConfiguration();
            builder.AddDataStorageModule();
            builder.AddStorageModule();
            builder.AddInfrastructureModule();
            builder.AddEventPublisher();
        }
    }
}
