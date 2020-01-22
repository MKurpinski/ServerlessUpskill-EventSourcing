using Upskill.Events;

namespace Category.Core.Events.Internal
{
    public class DeleteCategoryProcessStartedEvent : BaseEvent
    {
        public string Id { get; set; }

        public DeleteCategoryProcessStartedEvent(
            string id,
            string correlationId) : base(correlationId)
        {
            Id = id;
        }
    }
}