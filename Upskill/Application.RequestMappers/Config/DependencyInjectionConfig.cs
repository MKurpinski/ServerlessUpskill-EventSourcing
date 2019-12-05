using Application.RequestMappers.Dtos;
using Application.RequestMappers.Options;
using Application.RequestMappers.RequestToDtoMappers;
using Application.RequestMappers.RequestToDtoMappers.Implementation;
using Application.RequestMappers.Validators;
using FluentValidation;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.RequestMappers.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddRequestMappersModule(this IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<ApplicationFormValidationOptions>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.Bind(settings);
                });

            return builder.Services
                .AddTransient<IValidator<RegisterApplicationDto>, RegisterApplicationDtoValidator>()
                .AddTransient<IValidator<CandidateDto>, CandidateDtoValidator>()
                .AddTransient<IFromFormToApplicationAddDtoRequestMapper, FromFormToApplicationAddDtoRequestMapper>()
                .AddTransient<IFromFormToApplicationDtoDeserializer, FromFormToApplicationDtoDeserializer>();
        }
    }
}
