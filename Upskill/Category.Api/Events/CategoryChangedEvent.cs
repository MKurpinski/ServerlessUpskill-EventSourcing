using Upskill.EventPublisher;

namespace Category.Api.Events
{
    public class CategoryChangedEvent : IEvent
    {
        public string Id { get; }
        public string Name { get; }
        public string Description { get; }
        public int SortOrder { get; }

        public CategoryChangedEvent(string id, string name, string description, int sortOrder)
        {
            Id = id;
            Name = name;
            Description = description;
            SortOrder = sortOrder;
        }
    }
}
