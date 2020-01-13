using Upskill.Events;

namespace Application.Category.Events.Incoming
{
    public class CategoryChangedEvent : BaseEvent
    {
        public string Id { get; }
        public string Name { get; }

        public CategoryChangedEvent(
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
