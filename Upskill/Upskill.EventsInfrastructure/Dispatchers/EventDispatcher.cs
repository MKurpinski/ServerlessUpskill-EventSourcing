using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Upskill.Events;
using Upskill.EventsInfrastructure.Extensions;
using Upskill.EventsInfrastructure.Providers;

namespace Upskill.EventsInfrastructure.Dispatchers
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly ILogger<EventDispatcher> _logger;
        private readonly IEventTypeProvider _typeProvider;
        private readonly IHandlerImplementationProvider _serviceProvider;

        public EventDispatcher(
            ILogger<EventDispatcher> logger,
            IHandlerImplementationProvider serviceProvider, IEventTypeProvider typeProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _typeProvider = typeProvider;
        }

        public async Task Dispatch(params EventGridEvent[] events)
        {
            foreach (var @event in events)
            {
                var eventTypeResult = _typeProvider.ResolveEventType(@event.EventType);
                if (!eventTypeResult.Success)
                {
                    this.LogNotHandledEvent(@event);
                    continue;
                }

                var handlerType = typeof(IEventHandler<>).MakeGenericType(eventTypeResult.Value);
                var canHandle = _serviceProvider.TryResolveHandlers(handlerType, out var handlers);

                if (!canHandle)
                {
                    this.LogNotHandledEvent(@event);
                    continue;
                }

                var eventContent = JsonConvert.DeserializeObject(@event.Data as string, eventTypeResult.Value);
                await Task.WhenAll(handlers.Select(h => h.InvokeAsync(nameof(IEventHandler<IEvent>.Handle), eventContent)));
            }
        }

        private void LogNotHandledEvent(EventGridEvent @event)
        {
            _logger.LogError($"Cannot handle event of type {@event.EventType}. \nEvent: {@event}");
        }
    }
}