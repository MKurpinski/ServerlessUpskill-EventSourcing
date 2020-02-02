namespace Upskill.Events
{
    public interface IAggregateEvent : IEvent
    {
        string Id { get; }
    }
}
