using Upskill.Events;

namespace Category.Core.Events
{
    public class CategoryDeletedEvent : BaseEvent, IAggregateEvent
    {
        public string Id { get; }

        public CategoryDeletedEvent(
            string id,
            string correlationId) 
            : base(correlationId)
        {
            Id = id;
        }
    }
}
