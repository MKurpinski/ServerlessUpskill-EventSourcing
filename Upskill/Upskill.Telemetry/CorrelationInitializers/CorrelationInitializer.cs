using Upskill.Telemetry.Facades;

namespace Upskill.Telemetry.CorrelationInitializers
{
    public class CorrelationInitializer : ICorrelationInitializer
    {
        private readonly IActivityFacade _activityFacade;

        public CorrelationInitializer(IActivityFacade activityFacade)
        {
            _activityFacade = activityFacade;
        }

        public void Initialize(string correlationId)
        {
            _activityFacade.SetCorrelationId(correlationId);
        }
    }
}