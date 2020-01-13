using Upskill.Events;

namespace Category.Core.Events.External
{
    public class CategoryDeletedEvent : BaseEvent
    {
        public string Id { get; }

        public CategoryDeletedEvent(
            string id,
            string correlationId) 
            : base(correlationId)
        {
            Id = id;
        }
    }
}
