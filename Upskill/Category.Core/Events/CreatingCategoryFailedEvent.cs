using Category.Core.Enums;
using Upskill.Events;

namespace Category.Core.Events
{
    public class CreatingCategoryFailedEvent : BaseStatusEvent, IAggregateEvent
    {
        public string Id { get; }

        public CreatingCategoryFailedEvent(
            string id,
            CategoryModificationStatus status,
            string correlationId) 
            : base(status.ToString(), correlationId)
        {
            Id = id;
        }
    }
}
