using Upskill.Events;

namespace Category.Core.Events.External
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
