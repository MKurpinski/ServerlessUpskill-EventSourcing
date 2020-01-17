using Category.Core.Enums;
using Upskill.Events;

namespace Category.Core.Events.Internal
{
    public class DeletingCategoryFailedEvent : BaseEvent
    {
        public CategoryModificationStatus Status { get; }
        public DeletingCategoryFailedEvent(CategoryModificationStatus status, string correlationId) : base(correlationId)
        {
            Status = status;
        }
    }
}
