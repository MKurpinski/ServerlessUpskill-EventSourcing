using Upskill.Events;

namespace Category.Core.Events
{
    public class CategoryUpdatedEvent : BaseEvent
    {
        public string Id { get; }
        public string Name { get; }
        public string Description { get; }
        public int SortOrder { get; }

        public CategoryUpdatedEvent(
            string id, 
            string name,
            string description,
            int sortOrder,
            string correlationId) 
            : base(correlationId)
        {
            Id = id;
            Name = name;
            Description = description;
            SortOrder = sortOrder;
        }
    }
}
