using Category.Core.Enums;
using Upskill.Events;

namespace Category.Core.Events
{
    public class UpdatingCategoryFailedEvent : BaseStatusEvent, IAggregateEvent
    {
        public string Id { get; }

        public UpdatingCategoryFailedEvent(
            string id,
            CategoryModificationStatus status,
            string correlationId)
            : base(status.ToString(), correlationId)
        {
            Id = id;
        }
    }
}
