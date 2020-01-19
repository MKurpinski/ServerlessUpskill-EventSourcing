using Upskill.Events;

namespace Application.Core.Events
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
