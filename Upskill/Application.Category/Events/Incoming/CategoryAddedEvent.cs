namespace Application.Category.Events.Incoming
{
    public class CategoryAddedEvent : CategoryChangedEvent
    {
        public CategoryAddedEvent(
            string correlationId,
            string id, 
            string name) 
            : base(correlationId, id, name)
        {
        }
    }
}
