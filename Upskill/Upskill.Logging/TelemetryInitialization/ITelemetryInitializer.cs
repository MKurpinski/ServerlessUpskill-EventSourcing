namespace Upskill.Logging.TelemetryInitialization
{
    public interface ITelemetryInitializer
    {
        void Initialize(string correlationId);
    }
}
