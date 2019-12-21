using Upskill.EventPublisher;

namespace Application.Api.Events.External
{
    public class CategoryUsedEvent : IEvent
    {
        public CategoryUsedEvent(
            string name,
            string usedIn)
        {
            Name = name;
            UsedIn = usedIn;
        }

        public string Name { get; }
        public string UsedIn { get; }
    }
}
