namespace Upskill.Events
{
    public interface IEvent
    {
        string CorrelationId { get; }
    }
}
