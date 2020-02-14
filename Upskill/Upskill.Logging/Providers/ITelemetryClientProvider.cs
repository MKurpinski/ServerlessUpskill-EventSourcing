using Microsoft.ApplicationInsights;

namespace Upskill.Logging.Providers
{
    public interface ITelemetryClientProvider
    {
        TelemetryClient GetClient();
    }
}
