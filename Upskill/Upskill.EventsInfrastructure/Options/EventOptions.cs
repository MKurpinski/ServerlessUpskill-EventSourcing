using System.Collections.Generic;

namespace Upskill.EventsInfrastructure.Options
{
    public class EventOptions
    {
        public string DomainEndpoint { get; set; }
        public string DomainKey { get; set; }
        public IReadOnlyCollection<EventInformation> Events { get; set; }
    }
}
