using Application.RequestMappers.Dtos;
using Application.RequestMappers.Options;
using Application.RequestMappers.RequestToDtoMappers;
using Application.RequestMappers.RequestToDtoMappers.Implementation;
using Application.RequestMappers.Validators;
using FluentValidation;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Infrastructure.Extensions;

namespace Application.RequestMappers.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddRequestMappersModule(this IFunctionsHostBuilder builder)
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
            builder.ConfigureOptions<ApplicationFormValidationOptions>();

            return builder.Services
                .AddTransient<IValidator<RegisterApplicationDto>, RegisterApplicationDtoValidator>()
                .AddTransient<IValidator<CandidateDto>, CandidateDtoValidator>()
                .AddTransient<IValidator<AddressDto>, AddressDtoValidator>()
                .AddTransient<IValidator<ConfirmedSkillDto>, ConfirmedSkillDtoValidator>()
                .AddTransient<IValidator<WorkExperienceDto>, WorkExperienceDtoValidator>()
                .AddTransient<IValidator<FinishedSchoolDto>, FinishedSchoolDtoValidator>()
                .AddTransient<IFromFormToApplicationAddDtoRequestMapper, FromFormToApplicationAddDtoRequestMapper>()
                .AddTransient<IFromFormToApplicationDtoDeserializer, FromFormToApplicationDtoDeserializer>();
        }
    }
}
