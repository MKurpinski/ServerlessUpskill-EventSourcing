using Upskill.Events;

namespace Application.Core.Events
{
    public class CategoryNameChangedEvent : IEvent
    {
        public string OldName { get; set; }
        public string NewName { get; set; }
    }
}
