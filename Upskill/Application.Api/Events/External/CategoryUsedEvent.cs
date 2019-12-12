using Upskill.EventPublisher;

namespace Application.Api.Events.External
{
    public class CategoryUsedEvent : IEvent
    {
        public CategoryUsedEvent(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
