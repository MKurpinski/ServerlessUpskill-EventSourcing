using Upskill.Events;

namespace Application.Category.Events.Incoming
{
    public class CategoryChangedEvent : IEvent
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
