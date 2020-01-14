namespace Category.Core.Events.Internal
{
    public class CreateCategoryProcessStartedEvent : UpdateCategoryProcessStartedEvent
    {
        public CreateCategoryProcessStartedEvent(
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
