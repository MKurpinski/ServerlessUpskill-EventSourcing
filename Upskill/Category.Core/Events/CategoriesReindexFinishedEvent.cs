using Upskill.Events;

namespace Category.Core.Events
{
    public class CategoriesReindexFinishedEvent : BaseEvent
    {
        public CategoriesReindexFinishedEvent(string correlationId) : base(correlationId)
        {
        }
    }
}
