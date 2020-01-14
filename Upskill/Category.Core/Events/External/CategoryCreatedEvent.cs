namespace Category.Core.Events.External
{
    public class CategoryCreatedEvent : CategoryUpdatedEvent
    {
        public CategoryCreatedEvent(
            string id,
            string name,
            string description,
            int sortOrder,
            string correlationId) 
            : base(id, name, description, sortOrder, correlationId)
        {
        }
    }
}
