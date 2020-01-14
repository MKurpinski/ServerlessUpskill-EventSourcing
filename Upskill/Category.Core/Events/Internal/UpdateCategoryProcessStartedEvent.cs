using Upskill.Events;

namespace Category.Core.Events.Internal
{
    public class UpdateCategoryProcessStartedEvent : BaseEvent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }

        public UpdateCategoryProcessStartedEvent(
            string id, 
            string name,
            string description,
            int sortOrder,
            string correlationId) : base(correlationId)
        {
            Id = id;
            Name = name;
            Description = description;
            SortOrder = sortOrder;
        }
    }
}
