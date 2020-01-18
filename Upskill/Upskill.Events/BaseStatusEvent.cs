namespace Upskill.Events
{
    public abstract class BaseStatusEvent : BaseEvent, IBaseStatusEvent
    {
        public string Status { get; }

        protected BaseStatusEvent(
            string status,
            string correlationId) : base(correlationId)
        {
            Status = status;
        }
    }
}
