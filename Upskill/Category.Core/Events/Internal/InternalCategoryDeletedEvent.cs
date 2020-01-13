using Upskill.Events;

namespace Category.Core.Events.Internal
{
    public class InternalCategoryDeletedEvent : BaseEvent
    {
        public string Id { get; set; }

        public InternalCategoryDeletedEvent(
            string id,
            string correlationId) : base(correlationId)
        {
            Id = id;
        }
    }
}
