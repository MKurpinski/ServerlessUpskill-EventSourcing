using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace Upskill.Logging.Providers
{
    public class TelemetryClientProvider : ITelemetryClientProvider
    {
        private readonly TelemetryClient _client;
        public TelemetryClientProvider(TelemetryConfiguration telemetryConfiguration)
        {
            _client = new TelemetryClient(telemetryConfiguration);
        }

        public TelemetryClient GetClient() => _client;
    }
}