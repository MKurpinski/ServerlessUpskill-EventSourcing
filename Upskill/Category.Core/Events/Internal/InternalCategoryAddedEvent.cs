namespace Category.Core.Events.Internal
{
    public class InternalCategoryAddedEvent : InternalCategoryChangedEvent
    {
        public InternalCategoryAddedEvent(
            string id,
            string name,
            string description,
            int sortOrder) 
            : base(id, name, description, sortOrder)
        {
        }
    }
}
