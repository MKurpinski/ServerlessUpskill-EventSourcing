using Category.Core.Enums;
using Upskill.Events;

namespace Category.Core.Events.Internal
{
    public class UpdatingCategoryFailedEvent : BaseEvent
    {
        public CategoryModificationStatus Reason { get; }
        public UpdatingCategoryFailedEvent(CategoryModificationStatus reason, string correlationId) : base(correlationId)
        {
            Reason = reason;
        }
    }
}
