namespace Upskill.Events
{
    public interface IBuildBy<TEvent> where TEvent : IEvent
    {
        void ApplyEvent(TEvent @event);
    }
}
