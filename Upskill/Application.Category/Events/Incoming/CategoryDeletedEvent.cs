using Upskill.Events;

namespace Application.Category.Events.Incoming
{
    public class CategoryDeletedEvent : BaseEvent
    {
        public string Id { get; }

        public CategoryDeletedEvent(
            string correlationId,
            string id) 
            : base(correlationId)
        {
            Id = id;
        }
    }
}
