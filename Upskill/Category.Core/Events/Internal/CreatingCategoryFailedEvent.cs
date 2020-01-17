using Category.Core.Enums;
using Upskill.Events;

namespace Category.Core.Events.Internal
{
    public class CreatingCategoryFailedEvent : BaseEvent
    {
        public CategoryModificationStatus Status { get; }
        public CreatingCategoryFailedEvent(CategoryModificationStatus status, string correlationId) : base(correlationId)
        {
            Status = status;
        }
    }
}
