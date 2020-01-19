using Category.Core.Enums;
using Upskill.Events;

namespace Category.Core.Events.Internal
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
