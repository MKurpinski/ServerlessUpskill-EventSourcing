using Microsoft.ApplicationInsights;
using Upskill.Logging.Providers;

namespace Upskill.Logging.TelemetryInitialization
{
    public class TelemetryInitializer : ITelemetryInitializer
    {
        private readonly TelemetryClient _client;

        public TelemetryInitializer(ITelemetryClientProvider clientProvider)
        {
            _client = clientProvider.GetClient();
        }

        public void Initialize(string correlationId)
        {
            _client.Context.Operation.ParentId = correlationId;
        }
    }
}