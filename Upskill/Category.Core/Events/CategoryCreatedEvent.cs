using Upskill.Events;

namespace Category.Core.Events
{
    public class CategoryCreatedEvent : CategoryUpdatedEvent, IAggregateEvent
    {
        public CategoryCreatedEvent(
            string id,
            string name,
            string description,
            int sortOrder,
            string correlationId) 
            : base(id, name, description, sortOrder, correlationId)
        {
        }
    }
}
