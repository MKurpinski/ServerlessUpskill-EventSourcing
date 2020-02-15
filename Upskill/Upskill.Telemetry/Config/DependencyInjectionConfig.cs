using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Upskill.Telemetry.CorrelationInitializers;
using Upskill.Telemetry.Facades;
using Upskill.Telemetry.TelemetryInitializers;

namespace Upskill.Telemetry.Config
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddTelemetryLogging(this IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ITelemetryInitializer, CorrelatingTelemetryInitializer>();
            return builder.Services
                .AddTransient<ICorrelationInitializer, CorrelationInitializer>()
                .AddTransient<IActivityFacade, ActivityFacade>();
        }
    }
}
