using Application.Api.CommandBuilders;
using Application.Api.Commands.RegisterApplicationCommand;
using Application.Api.Dtos;
using Application.Api.Options;
using Application.Api.RequestToDtoMappers;
using Application.Api.RequestToDtoMappers.Implementation;
using Application.Api.Utils;
using Application.Api.Utils.Implementation;
using Application.Api.Validators;
using Application.Infrastructure;
using Application.Infrastructure.Implementation;
using FluentValidation;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Application.Api.Startup))]

namespace Application.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IGuidProvider, GuidProvider>();
            builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddTransient<IFromFormToApplicationAddDtoRequestMapper, FromFormToApplicationAddDtoRequestMapper>();
            builder.Services.AddTransient<IFromFormToApplicationDtoDeserializer, FromFormToApplicationDtoDeserializer>();
            builder.Services.AddTransient<IFileToByteArrayConverter, FileToByteArrayConverter>();
            builder.Services.AddTransient<ICommandBuilder<RegisterApplicationDto, RegisterApplicationCommand>, RegisterApplicationCommandBuilder>();
            builder.Services.AddTransient<IValidator<RegisterApplicationDto>, RegisterApplicationDtoValidator>();
            builder.Services.AddTransient<IValidator<CandidateDto>, CandidateDtoValidator>();

            builder.Services.AddOptions<ApplicationFormOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.Bind(settings);
                });
        }
    }
}
