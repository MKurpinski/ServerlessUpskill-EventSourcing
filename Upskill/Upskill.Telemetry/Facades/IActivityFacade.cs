using Upskill.Results;

namespace Upskill.Telemetry.Facades
{
    public interface IActivityFacade
    {
        void SetCorrelationId(string correlationId);
        IDataResult<string> GetCorrelationId();
    }
}
