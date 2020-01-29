using Upskill.Events;

namespace Application.Core.Events
{
    public class ApplicationReindexFinishedEvent : BaseEvent
    {
        public ApplicationReindexFinishedEvent(string correlationId) : base(correlationId)
        {
        }
    }
}
