using Upskill.Events;

namespace Application.Core.Events
{
    public class ApplicationCategoryNameChangedEvent : BaseEvent, IAggregateEvent
    {
        public string Id { get; }
        public string NewCategoryName { get; }

        public ApplicationCategoryNameChangedEvent(
            string id, 
            string newCategoryName,
            string correlationId) : base(correlationId)
        {
            Id = id;
            NewCategoryName = newCategoryName;
        }
    }
}
