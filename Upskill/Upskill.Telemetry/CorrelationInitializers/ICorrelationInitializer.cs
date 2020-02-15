namespace Upskill.Telemetry.CorrelationInitializers
{
    public interface ICorrelationInitializer
    {
        void Initialize(string correlationId);
    }
}
