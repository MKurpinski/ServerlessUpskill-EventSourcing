using Upskill.Events;

namespace Category.Core.Events
{
    public class CategoryUsedEvent : BaseEvent
    {
        public string Name { get; }
        public string UsedIn { get; }

        public CategoryUsedEvent(
            string name,
            string usedIn,
            string correlationId) 
            : base(correlationId)
        {
            Name = name;
            UsedIn = usedIn;
        }
    }
}
