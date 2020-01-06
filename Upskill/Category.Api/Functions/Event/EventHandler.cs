// http://localhost:7071/runtime/webhooks/EventGrid?functionName=EventHandler

using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Upskill.EventsInfrastructure.Dispatchers;

namespace Category.Api.Functions.Event
{
    public class EventHandler
    {
        private readonly IEventDispatcher _eventDispatcher;

        public EventHandler(IEventDispatcher eventDispatcher)
        {
            _eventDispatcher = eventDispatcher;
        }

        [FunctionName(nameof(EventHandler))]
        public async Task Run([EventGridTrigger]EventGridEvent eventGridEvent)
        {
            await _eventDispatcher.Dispatch(eventGridEvent);
        }
    }
}
