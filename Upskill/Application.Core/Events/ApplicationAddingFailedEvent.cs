using Upskill.Events;

namespace Application.Core.Events
{
    public class ApplicationAddingFailedEvent : BaseEvent
    {
        public ApplicationAddingFailedEvent(string correlationId) : base(correlationId)
        {
        }
    }
}
