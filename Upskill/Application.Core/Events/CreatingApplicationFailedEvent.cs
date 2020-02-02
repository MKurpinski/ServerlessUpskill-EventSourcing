using Upskill.Events;

namespace Application.Core.Events
{
    public class CreatingApplicationFailedEvent : BaseStatusEvent, IAggregateEvent
    {
        public string Id { get; }
        public CreatingApplicationFailedEvent(string id, string status, string correlationId) 
            : base(status, correlationId)
        {
            Id = id;
        }
    }
}
