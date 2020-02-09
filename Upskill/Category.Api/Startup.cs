using Category.Api.Commands;
using Category.Api.CustomHttpRequests;
using Category.Api.Validators;
using Category.Core.Config;
using Category.Search.Config;
using Category.Storage.Config;
using FluentValidation;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.EventsInfrastructure.Config;
using Upskill.FunctionUtils.Extensions;
using Upskill.Infrastructure.Config;
using Upskill.LogChecker.Config;
using Upskill.RealTimeNotifications.Config;

[assembly: FunctionsStartup(typeof(Category.Api.Startup))]

namespace Category.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IValidator<CreateCategoryHttpRequest>, CreateCategoryHttpRequestValidator>();
            builder.Services.AddTransient<IValidator<UpdateCategoryCommand>, UpdateCategoryCommandValidator>();
            builder.Services.AddTransient<IValidator<GetCategoriesHttpRequest>, GetCategoriesHttpRequestValidator>();

            builder.AddRealTimeNotifications();
            builder.AddCoreModule();
            builder.AddSearchModule();
            builder.AddAppSettingsToConfiguration();
            builder.AddStorageModule();
            builder.AddInfrastructureModule();
            builder.AddEvents();
            builder.AddLogChecker();
        }
    }
}
