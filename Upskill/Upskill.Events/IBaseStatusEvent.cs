namespace Upskill.Events
{
    public interface IBaseStatusEvent : IEvent
    {
        string Status { get; }
    }
}
