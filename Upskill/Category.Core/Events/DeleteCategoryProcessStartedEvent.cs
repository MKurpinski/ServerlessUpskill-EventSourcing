using Upskill.Events;

namespace Category.Core.Events
{
    public class DeleteCategoryProcessStartedEvent : BaseEvent, IAggregateEvent
    {
        public string Id { get; set; }

        public DeleteCategoryProcessStartedEvent(
            string id,
            string correlationId) : base(correlationId)
        {
            Id = id;
        }
    }
}