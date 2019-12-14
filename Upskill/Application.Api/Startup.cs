﻿using Application.Api.Extensions;
using Application.Api.Profiles;
using Application.Commands.Config;
using Application.Commands.Profiles;
using Application.DataStorage.Config;
using Application.ProcessStatus.Config;
using Application.RequestMappers.Config;
using Application.Search.Config;
using Application.Search.Profiles;
using Application.Storage.Config;
using AutoMapper;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Upskill.EventPublisher.Config;
using Upskill.Infrastructure.Config;

[assembly: FunctionsStartup(typeof(Application.Api.Startup))]

namespace Application.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAutoMapper(
                typeof(SaveApplicationCommandToApplicationProfile).Assembly,
                typeof(SaveApplicationCommandToApplicationProfile).Assembly,
                typeof(CandidateDtoToCandidateProfile).Assembly,
                typeof(SearchableApplicationToApplicationDtoProfile).Assembly);

            builder.AddAppSettingsToConfiguration();
            builder.AddRequestMappersModule();
            builder.AddInfrastructureModule();
            builder.AddCommandsModule();
            builder.AddStorageModule();
            builder.AddDataStorageModule();
            builder.AddProcessStatusModule();
            builder.AddSearchModule();
            builder.AddEventPublisher();
        }
    }
}
