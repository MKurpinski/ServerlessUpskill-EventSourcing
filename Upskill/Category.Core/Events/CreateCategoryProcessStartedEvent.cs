using Upskill.Events;

namespace Category.Core.Events
{
    public class CreateCategoryProcessStartedEvent : UpdateCategoryProcessStartedEvent, IAggregateEvent
    {
        public CreateCategoryProcessStartedEvent(
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
