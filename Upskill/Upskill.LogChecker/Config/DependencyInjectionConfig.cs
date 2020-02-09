using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Infrastructure.Extensions;
using Upskill.LogChecker.Clients;
using Upskill.LogChecker.Options;
using Upskill.LogChecker.Providers;

namespace Upskill.LogChecker.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddLogChecker(this IFunctionsHostBuilder builder)
        {
            builder.ConfigureOptions<LogAnalyticsOptions>();

            return builder.Services
                .AddTransient<ILastLogProvider, LastLogProvider>()
                .AddSingleton<ILogAnalyticsClientFactory, LogAnalyticsClientFactory>();
        }
    }
}
