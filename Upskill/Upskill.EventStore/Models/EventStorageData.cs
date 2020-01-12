namespace Upskill.EventStore.Models
{
    public class EventStorageData
    {
        public string Id { get; }
        public string EventType { get; }
        public string StreamType { get; }
        public string Content { get; }

        public EventStorageData(string streamType, string id, string eventType, string content)
        {
            Id = id;
            EventType = eventType;
            Content = content;
            StreamType = streamType;
        }
    }
}
