namespace Upskill.ReindexGuards
{
    public class PendingEvent
    {
        public string Content { get; }
        public string EventType { get; }

        public PendingEvent(string content, string eventType)
        {
            Content = content;
            EventType = eventType;
        }
    }
}
