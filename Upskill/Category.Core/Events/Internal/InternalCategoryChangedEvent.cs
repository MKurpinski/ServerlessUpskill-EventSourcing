using Upskill.Events;

namespace Category.Core.Events.Internal
{
    public class InternalCategoryChangedEvent : IEvent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }

        public InternalCategoryChangedEvent(string id, string name, string description, int sortOrder)
        {
            Id = id;
            Name = name;
            Description = description;
            SortOrder = sortOrder;
        }
    }
}
