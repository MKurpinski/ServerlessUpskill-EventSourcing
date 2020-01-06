using Upskill.Events;

namespace Application.Category.Events.Incoming
{
    public class CategoryDeletedEvent : IEvent
    {
        public string Id { get; set; }
    }
}
