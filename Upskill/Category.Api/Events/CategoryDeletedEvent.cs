using Upskill.EventPublisher;

namespace Category.Api.Events
{
    public class CategoryDeletedEvent : IEvent
    {
        public string Id { get; }

        public CategoryDeletedEvent(string id)
        {
            Id = id;
        }
    }
}
