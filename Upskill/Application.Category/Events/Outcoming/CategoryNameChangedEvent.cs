using Upskill.Events;

namespace Application.Category.Events.Outcoming
{
    public class CategoryNameChangedEvent : BaseEvent
    {
        public string OldName { get; }
        public string NewName { get; }

        public CategoryNameChangedEvent(
            string oldName,
            string newName,
            string correlationId)
            : base(correlationId)
        {
            OldName = oldName;
            NewName = newName;
        }
    }
}
