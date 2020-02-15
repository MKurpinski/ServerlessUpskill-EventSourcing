using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Upskill.Telemetry.Facades;

namespace Upskill.Telemetry.TelemetryInitializers
{
    public class CorrelatingTelemetryInitializer : OperationCorrelationTelemetryInitializer, ITelemetryInitializer
    {
        private readonly IActivityFacade _activityFacade;

        public CorrelatingTelemetryInitializer(IActivityFacade activityFacade)
        {
            _activityFacade = activityFacade;
        }

        void ITelemetryInitializer.Initialize(ITelemetry telemetry)
        {
            base.Initialize(telemetry);

            var correlationIdResult = _activityFacade.GetCorrelationId();

            if (!correlationIdResult.Success)
            {
                return;
            }

            telemetry.Context.Operation.ParentId = correlationIdResult.Value;
        }
    }
}
