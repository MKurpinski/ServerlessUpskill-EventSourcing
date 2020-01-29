using Upskill.Events;

namespace Category.Core.Events
{
    public class CategoriesReindexStartedEvent : BaseEvent
    {
        public CategoriesReindexStartedEvent(string correlationId) : base(correlationId)
        {
        }
    }
}
