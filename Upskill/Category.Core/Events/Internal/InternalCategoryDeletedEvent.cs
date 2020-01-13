using Upskill.Events;

namespace Category.Core.Events.Internal
{
    public class InternalCategoryDeletedEvent : IEvent
    {
        public string Id { get; set; }

        public InternalCategoryDeletedEvent(string id)
        {
            Id = id;
        }
    }
}
