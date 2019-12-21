using Upskill.EventPublisher;

namespace Application.Api.Events.External.Category
{
    public class CategoryChangedEvent : IEvent
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
