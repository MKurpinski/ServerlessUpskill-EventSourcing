
using Upskill.EventPublisher;

namespace Category.Api.Events
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
