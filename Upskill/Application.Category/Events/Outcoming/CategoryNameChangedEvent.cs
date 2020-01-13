using Upskill.Events;

namespace Application.Category.Events.Outcoming
{
    public class CategoryNameChangedEvent : IEvent
    {
        public string OldName { get; }
        public string NewName { get; }

        public CategoryNameChangedEvent(string oldName, string newName)
        {
            OldName = oldName;
            NewName = newName;
        }
    }
}
