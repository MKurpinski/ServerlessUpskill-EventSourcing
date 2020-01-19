namespace Upskill.Events
{
    public abstract class BaseEvent : IEvent
    {
        public string CorrelationId { get; }

        protected BaseEvent(string correlationId)
        {
            CorrelationId = correlationId;
        }
    }
}
