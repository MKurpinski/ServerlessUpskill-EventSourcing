using Upskill.Events;

namespace Application.Core.Events
{
    public class CreatingApplicationFailedEvent : BaseStatusEvent
    {
        public CreatingApplicationFailedEvent(string status, string correlationId) 
            : base(status, correlationId)
        {
        }
    }
}
