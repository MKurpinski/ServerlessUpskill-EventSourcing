using Category.Core.Enums;
using Upskill.Events;

namespace Category.Core.Events
{
    public class DeletingCategoryFailedEvent : BaseStatusEvent, IAggregateEvent
    {
        public string Id { get; }

        public DeletingCategoryFailedEvent(
            string id,
            CategoryModificationStatus status,
            string correlationId)
            : base(status.ToString(), correlationId)
        {
            Id = id;
        }
    }
}
