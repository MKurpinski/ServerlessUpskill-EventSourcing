using Upskill.EventPublisher;

namespace Application.Api.Events.External.Category
{
    public class CategoryDeletedEvent : IEvent
    {
        public string Id { get; set; }
    }
}
