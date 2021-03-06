﻿using Application.Api.CustomHttpRequests;
using Application.Api.Profiles;
using Application.Api.Validators;
using Application.Category.Config;
using Application.Commands.Config;
using Application.Commands.Profiles;
using Application.Core.Config;
using Application.Core.Profiles;
using Application.ProcessStatus.Config;
using Application.PushNotifications.Config;
using Application.RequestMappers.Config;
using Application.Search.Config;
using Application.Search.Profiles;
using Application.Storage.Config;
using AutoMapper;
using FluentValidation;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Upskill.EventsInfrastructure.Config;
using Upskill.FunctionUtils.Extensions;
using Upskill.Infrastructure.Config;
using Upskill.LogChecker.Config;
using Upskill.RealTimeNotifications.Config;
using Upskill.Telemetry.Config;

[assembly: FunctionsStartup(typeof(Application.Api.Startup))]

namespace Application.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging(options =>
            {
                options.AddFilter("Upskill", LogLevel.Information);
                options.AddFilter("Application", LogLevel.Information);
            });

            builder.Services.AddAutoMapper(
                typeof(CreateApplicationCommandToCreateApplicationProcessStartedEventProfile).Assembly,
                typeof(CreateApplicationProcessStartedEventToApplicationDtoProfile).Assembly,
                typeof(CandidateDtoToCandidateProfile).Assembly,
                typeof(SearchableApplicationToApplicationDtoProfile).Assembly);

            builder.Services.AddTransient<IValidator<SimpleApplicationSearchHttpRequest>, SimpleApplicationSearchHttpRequestValidator>();
            builder.AddTelemetryLogging();
            builder.AddRealTimeNotifications();
            builder.AddAppSettingsToConfiguration();
            builder.AddCoreModule();
            builder.AddRequestMappersModule();
            builder.AddInfrastructureModule();
            builder.AddCommandsModule();
            builder.AddStorageModule();
            builder.AddProcessStatusModule();
            builder.AddSearchModule();
            builder.AddCategories();
            builder.AddEvents();
            builder.AddPushNotifications();
            builder.AddLogChecker();
        }
    }
}
