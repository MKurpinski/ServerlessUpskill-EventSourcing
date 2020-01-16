using Category.Core.Enums;
using Upskill.Events;

namespace Category.Core.Events.Internal
{
    public class CreatingCategoryFailedEvent : BaseEvent
    {
        public CategoryModificationStatus Reason { get; }
        public CreatingCategoryFailedEvent(CategoryModificationStatus reason, string correlationId) : base(correlationId)
        {
            Reason = reason;
        }
    }
}
