using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;

namespace Upskill.EventPublisher.Clients
{
    public class EventGridClientFacade  : IEventGridClientFacade
    {
        public Task PublishEvent(TopicCredentials credentials, string endpoint, EventGridEvent eventGridEvent)
        {
            var client = new EventGridClient(credentials);
            return client.PublishEventsAsync(endpoint, new List<EventGridEvent> { eventGridEvent });
        }
    }
}
