using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Application.Infrastructure.Extensions
{
    public static class FunctionsHostBuilderExtensions
    {
        public static OptionsBuilder<T> ConfigureOptions<T>(this IFunctionsHostBuilder builder) where T: class
        {
            return builder.Services.AddOptions<T>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection(typeof(T).Name).Bind(settings);
                });
        }
    }
}
