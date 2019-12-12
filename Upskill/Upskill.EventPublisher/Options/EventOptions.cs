using System.Collections.Generic;

namespace Upskill.EventPublisher.Options
{
    public class EventOptions
    {
        public string TopicEndpointPattern { get; set; }
        public IReadOnlyCollection<TopicInformation> Topics { get; set; }
    }
}
