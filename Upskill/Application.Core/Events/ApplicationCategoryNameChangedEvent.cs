using Upskill.Events;

namespace Application.Core.Events
{
    public class ApplicationCategoryNameChangedEvent : IEvent
    {
        public string Id { get; }
        public string NewCategoryName { get; }

        public ApplicationCategoryNameChangedEvent(string id, string newCategoryName)
        {
            Id = id;
            NewCategoryName = newCategoryName;
        }
    }
}
