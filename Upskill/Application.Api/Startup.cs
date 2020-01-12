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
using Upskill.EventsInfrastructure.Config;
using Upskill.EventStore.Config;
using Upskill.FunctionUtils.Extensions;
using Upskill.Infrastructure.Config;

[assembly: FunctionsStartup(typeof(Application.Api.Startup))]

namespace Application.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAutoMapper(
                typeof(SaveApplicationCommandToApplicationAddedEventProfile).Assembly,
                typeof(ApplicationAddedEventToApplicationDtoProfile).Assembly,
                typeof(CandidateDtoToCandidateProfile).Assembly,
                typeof(SearchableApplicationToApplicationDtoProfile).Assembly);

            builder.Services.AddTransient<IValidator<SimpleApplicationSearchHttpRequest>, SimpleApplicationSearchHttpRequestValidator>();

            builder.AddEventStore();
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
        }
    }
}
