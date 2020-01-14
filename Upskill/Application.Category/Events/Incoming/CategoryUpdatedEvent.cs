using Upskill.Events;

namespace Application.Category.Events.Incoming
{
    public class CategoryUpdatedEvent : BaseEvent
    {
        public string Id { get; }
        public string Name { get; }

        public CategoryUpdatedEvent(
            string correlationId,
            string id,
            string name) 
            : base(correlationId)
        {
            Id = id;
            Name = name;
        }
    }
}
