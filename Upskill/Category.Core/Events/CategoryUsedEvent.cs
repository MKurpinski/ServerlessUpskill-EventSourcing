using Upskill.Events;

namespace Category.Core.Events
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
