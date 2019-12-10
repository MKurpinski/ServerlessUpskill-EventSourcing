using Application.Infrastructure.Extensions;
using Application.RequestMappers.Dtos;
using Application.RequestMappers.Options;
using Application.RequestMappers.RequestToDtoMappers;
using Application.RequestMappers.RequestToDtoMappers.Implementation;
using Application.RequestMappers.Validators;
using FluentValidation;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Application.RequestMappers.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddRequestMappersModule(this IFunctionsHostBuilder builder)
        {
            builder.ConfigureOptions<ApplicationFormValidationOptions>();

            return builder.Services
                .AddTransient<IValidator<RegisterApplicationDto>, RegisterApplicationDtoValidator>()
                .AddTransient<IValidator<CandidateDto>, CandidateDtoValidator>()
                .AddTransient<IFromFormToApplicationAddDtoRequestMapper, FromFormToApplicationAddDtoRequestMapper>()
                .AddTransient<IFromFormToApplicationDtoDeserializer, FromFormToApplicationDtoDeserializer>();
        }
    }
}
