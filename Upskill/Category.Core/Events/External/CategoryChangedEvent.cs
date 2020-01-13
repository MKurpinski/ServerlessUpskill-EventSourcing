using Upskill.Events;

namespace Category.Core.Events.External
{
    public class CategoryChangedEvent : BaseEvent
    {
        public string Id { get; }
        public string Name { get; }
        public string Description { get; }
        public int SortOrder { get; }

        public CategoryChangedEvent(
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
