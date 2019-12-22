using Upskill.EventPublisher;

namespace Category.Api.Events
{
    public class CategoryUsedEvent : IEvent
    {
        public CategoryUsedEvent(string name, string usedIn)
        {
            Name = name;
            UsedIn = usedIn;
        }

        public string Name { get; }
        public string UsedIn { get; }
    }
}
