using Upskill.Events;

namespace Application.Core.Events
{
    public class RebuildOfReadModelRequestedEvent : BaseEvent
    {
        public string AggregateId { get; }

        public RebuildOfReadModelRequestedEvent(string aggregateId, string correlationId) : base(correlationId)
        {
            AggregateId = aggregateId;
        }
    }
}
