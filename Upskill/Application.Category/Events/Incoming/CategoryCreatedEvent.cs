namespace Application.Category.Events.Incoming
{
    public class CategoryCreatedEvent : CategoryUpdatedEvent
    {
        public CategoryCreatedEvent(
            string correlationId,
            string id, 
            string name) 
            : base(correlationId, id, name)
        {
        }
    }
}
