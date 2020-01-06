using Upskill.Events;

namespace Category.Core.Events
{
    public class CategoryDeletedEvent : IEvent
    {
        public string Id { get; set; }

        public CategoryDeletedEvent(string id)
        {
            Id = id;
        }
    }
}
