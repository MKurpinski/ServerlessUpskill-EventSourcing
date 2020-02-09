using Upskill.Events;

namespace Upskill.EventsInfrastructure.Dtos
{
    public class DispatchedEvent
    {
        public IEvent Content { get; }
        public string Type { get; }

        public DispatchedEvent(IEvent content, string type)
        {
            Content = content;
            Type = type;
        }
    }
}
