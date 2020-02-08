using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Upskill.Events;
using Upskill.EventsInfrastructure.Dtos;
using Upskill.EventsInfrastructure.Providers;
using Upskill.Infrastructure;

namespace Upskill.EventsInfrastructure.Dispatchers
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly ILogger<EventDispatcher> _logger;
        private readonly IEventTypeProvider _typeProvider;
        private readonly IHandlerImplementationProvider _serviceProvider;
        private readonly IInvokerProvider _invokerProvider;

        public EventDispatcher(
            ILogger<EventDispatcher> logger,
            IHandlerImplementationProvider serviceProvider,
            IEventTypeProvider typeProvider,
            IInvokerProvider invokerProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _typeProvider = typeProvider;
            _invokerProvider = invokerProvider;
        }

        public async Task<IReadOnlyCollection<DispatchedEvent>> Dispatch(params EventGridEvent[] events)
        {
            var dispatchedEvents = new List<DispatchedEvent>();
            foreach (var @event in events)
            {
                var eventTypeResult = _typeProvider.ResolveEventType(@event.EventType);
                if (!eventTypeResult.Success)
                {
                    this.LogNotHandledEvent(@event);
                    continue;
                }

                var eventContent = JsonConvert.DeserializeObject(@event.Data as string, eventTypeResult.Value);
                dispatchedEvents.Add(new DispatchedEvent(eventContent as IEvent, eventTypeResult.Value.AssemblyQualifiedName));

                var handlerType = typeof(IEventHandler<>).MakeGenericType(eventTypeResult.Value);
                var canHandle = _serviceProvider.TryResolveHandlers(handlerType, out var handlers);

                if (!canHandle)
                {
                    this.LogNotHandledEvent(@event);
                    continue;
                }

                await Task.WhenAll(handlers.Select(handler =>
                {
                    var invoker =
                        _invokerProvider.GetAsyncInvoker(handler, eventContent, nameof(IEventHandler<IEvent>.Handle));
                    return invoker(handler, eventContent);
                }));
            }

            return dispatchedEvents;
        }

        private void LogNotHandledEvent(EventGridEvent @event)
        {
            _logger.LogError($"Cannot handle event of type {@event.EventType}. \nEvent: {@event}");
        }
    }
}