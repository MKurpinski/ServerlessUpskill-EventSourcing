using Category.Core.Enums;
using Upskill.Events;

namespace Category.Core.Events.Internal
{
    public class DeletingCategoryFailedEvent : BaseStatusEvent
    {
        public DeletingCategoryFailedEvent(
            CategoryModificationStatus status,
            string correlationId)
            : base(status.ToString(), correlationId)
        {
        }
    }
}
