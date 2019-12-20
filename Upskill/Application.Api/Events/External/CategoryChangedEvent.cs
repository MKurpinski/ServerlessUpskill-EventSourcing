using Upskill.EventPublisher;

namespace Application.Api.Events.External
{
    public class CategoryChangedEvent : IEvent
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
    }
}
