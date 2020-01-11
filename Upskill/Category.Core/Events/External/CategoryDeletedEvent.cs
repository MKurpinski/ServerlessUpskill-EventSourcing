using Upskill.Events;

namespace Category.Core.Events.External
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
