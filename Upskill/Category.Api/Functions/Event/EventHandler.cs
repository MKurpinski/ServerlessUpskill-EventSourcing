// http://localhost:7071/runtime/webhooks/EventGrid?functionName=EventHandler

using System.Threading.Tasks;
using Category.Api.Functions.Category.RebuildReadModel;
using Category.Core.Guards;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Upskill.EventsInfrastructure.Dispatchers;
using Upskill.ReindexGuards;
using Upskill.Telemetry.CorrelationInitializers;

namespace Category.Api.Functions.Event
{
    public class EventHandler
    {
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ICategoryQueueingEventGuard _queueingEventGuard;
        private readonly ICorrelationInitializer _correlationInitializer;

        public EventHandler(
            IEventDispatcher eventDispatcher,
            ICategoryQueueingEventGuard queueingEventGuard,
            ICorrelationInitializer correlationInitializer)
        {
            _eventDispatcher = eventDispatcher;
            _queueingEventGuard = queueingEventGuard;
            _correlationInitializer = correlationInitializer;
        }

        [FunctionName(nameof(EventHandler))]
        public async Task Run(
            [EventGridTrigger] EventGridEvent eventGridEvent,
            [DurableClient] IDurableEntityClient client)
        {
            _correlationInitializer.Initialize(eventGridEvent.Subject);
            var dispatchedEvents = await _eventDispatcher.Dispatch(eventGridEvent);

            foreach (var dispatchedEvent in dispatchedEvents)
            {
                var shouldQueueResult =
                    await _queueingEventGuard.ShouldQueueEvent(dispatchedEvent.Content);
                if (shouldQueueResult.Success)
                {
                    await client.SignalEntityAsync<ICategoryEntity>(shouldQueueResult.Value,
                        p => p.QueueEvent(new PendingEvent(
                            eventGridEvent.Data as string,
                            dispatchedEvent.Type,
                            eventGridEvent.EventTime)));
                }
            }
        }
    }
}
