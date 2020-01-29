using Upskill.Events;

namespace Application.Core.Events
{
    public class ApplicationReindexStartedEvent : BaseEvent
    {
        public ApplicationReindexStartedEvent(string correlationId) : base(correlationId)
        {
        }
    }
}
