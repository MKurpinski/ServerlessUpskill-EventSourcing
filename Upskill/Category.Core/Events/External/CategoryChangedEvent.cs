using Upskill.Events;

namespace Category.Core.Events.External
{
    public class CategoryChangedEvent : IEvent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }

        public CategoryChangedEvent(string id, string name, string description, int sortOrder)
        {
            Id = id;
            Name = name;
            Description = description;
            SortOrder = sortOrder;
        }
    }
}
