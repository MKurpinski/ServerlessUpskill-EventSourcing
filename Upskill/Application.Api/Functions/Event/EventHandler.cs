// http://localhost:7071/runtime/webhooks/EventGrid?functionName=EventHandler

using System.Threading.Tasks;
using Application.Api.Functions.RebuildReadModel;
using Application.Core.Guards;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Upskill.EventsInfrastructure.Dispatchers;

namespace Application.Api.Functions.Event
{
    public class EventHandler
    {
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IApplicationQueueingEventGuard _queueingEventGuard;

        public EventHandler(
            IEventDispatcher eventDispatcher,
            IApplicationQueueingEventGuard queueingEventGuard)
        {
            _eventDispatcher = eventDispatcher;
            _queueingEventGuard = queueingEventGuard;
        }

        [FunctionName(nameof(EventHandler))]
        public async Task Run(
            [EventGridTrigger] EventGridEvent eventGridEvent,
            [DurableClient] IDurableEntityClient client)
        {
            var dispatchedEvents = await _eventDispatcher.Dispatch(eventGridEvent);
            foreach (var dispatchedEvent in dispatchedEvents)
            {
                var shouldQueueResult =
                    await _queueingEventGuard.ShouldQueueEvent(dispatchedEvent);
                if (shouldQueueResult.Success)
                {
                    await client.SignalEntityAsync<IApplicationEntity>(shouldQueueResult.Value,
                        p => p.QueueEvent(dispatchedEvent));
                }
            }
        }
    }
}
