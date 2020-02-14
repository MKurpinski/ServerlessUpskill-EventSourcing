using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Logging.Providers;
using Upskill.Logging.TelemetryInitialization;

namespace Upskill.Logging.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddTelemetryLogging(this IFunctionsHostBuilder builder)
        {
            return builder.Services
                .AddTransient<ITelemetryClientProvider, TelemetryClientProvider>()
                .AddTransient<ITelemetryInitializer, TelemetryInitializer>();
        }
    }
}
