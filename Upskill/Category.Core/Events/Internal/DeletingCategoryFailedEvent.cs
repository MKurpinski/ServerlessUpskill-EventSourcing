using Category.Core.Enums;
using Upskill.Events;

namespace Category.Core.Events.Internal
{
    public class DeletingCategoryFailedEvent : BaseEvent
    {
        public CategoryModificationStatus Reason { get; }
        public DeletingCategoryFailedEvent(CategoryModificationStatus reason, string correlationId) : base(correlationId)
        {
            Reason = reason;
        }
    }
}
