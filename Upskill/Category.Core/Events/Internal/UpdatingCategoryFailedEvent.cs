using Category.Core.Enums;
using Upskill.Events;

namespace Category.Core.Events.Internal
{
    public class UpdatingCategoryFailedEvent : BaseEvent
    {
        public CategoryModificationStatus Status { get; }
        public UpdatingCategoryFailedEvent(CategoryModificationStatus status, string correlationId) : base(correlationId)
        {
            Status = status;
        }
    }
}
