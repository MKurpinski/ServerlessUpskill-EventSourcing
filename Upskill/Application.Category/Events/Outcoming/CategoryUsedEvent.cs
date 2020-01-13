using Upskill.Events;

namespace Application.Category.Events.Outcoming
{
    public class CategoryUsedEvent : BaseEvent
    {
        public CategoryUsedEvent(
            string name,
            string usedIn, 
            string correlationId)
        :base(correlationId)
        {
            Name = name;
            UsedIn = usedIn;
        }

        public string Name { get; }
        public string UsedIn { get; }
    }
}
