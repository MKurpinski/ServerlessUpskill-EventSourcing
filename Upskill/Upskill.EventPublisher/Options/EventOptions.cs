using System.Collections.Generic;

namespace Upskill.EventPublisher.Options
{
    public class EventOptions
    {
        public string DomainEndpointPattern { get; set; }
        public string DomainName { get; set; }
        public string RegionName { get; set; }
        public string DomainKey { get; set; }
        public IReadOnlyCollection<EventInformation> Topics { get; set; }
    }
}
