using Application.Commands.CommandBuilders;
using Application.Commands.Commands;
using Application.Commands.Utils;
using Application.RequestMappers.Dtos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Commands.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddCommandsModule(this IFunctionsHostBuilder builder)
        {
            return builder.Services
                .AddTransient<IFileToByteArrayConverter, FileToByteArrayConverter>()
                .AddTransient<ICommandBuilder<RegisterApplicationDto, RegisterApplicationCommand>, RegisterApplicationCommandBuilder>();
        }
    }
}
