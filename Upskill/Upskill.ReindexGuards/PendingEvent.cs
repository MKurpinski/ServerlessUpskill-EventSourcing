using System;

namespace Upskill.ReindexGuards
{
    public class PendingEvent
    {
        public string Content { get; }
        public string EventType { get; }
        public DateTime Timestamp { get; }

        public PendingEvent(
            string content,
            string eventType, 
            DateTime timestamp)
        {
            Content = content;
            EventType = eventType;
            Timestamp = timestamp;
        }
    }
}
