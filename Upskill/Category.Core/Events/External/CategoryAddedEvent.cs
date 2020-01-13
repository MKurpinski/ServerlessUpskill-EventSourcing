namespace Category.Core.Events.External
{
    public class CategoryAddedEvent : CategoryChangedEvent
    {
        public CategoryAddedEvent(
            string id,
            string name,
            string description,
            int sortOrder) 
            : base(id, name, description, sortOrder)
        {
        }
    }
}
