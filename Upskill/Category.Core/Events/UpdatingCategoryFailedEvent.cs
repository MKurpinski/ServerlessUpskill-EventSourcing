using Category.Core.Enums;
using Upskill.Events;

namespace Category.Core.Events
{
    public class UpdatingCategoryFailedEvent : BaseStatusEvent
    {
        public UpdatingCategoryFailedEvent(
            CategoryModificationStatus status,
            string correlationId)
            : base(status.ToString(), correlationId)
        {
        }
    }
}
