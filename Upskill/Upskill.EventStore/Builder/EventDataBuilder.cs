using Newtonsoft.Json;
using Streamstone;
using Upskill.Events;
using Upskill.EventStore.Models;
using Upskill.Infrastructure;

namespace Upskill.EventStore.Builder
{
    public class EventDataBuilder : IEventDataBuilder
    {
        private readonly IGuidProvider _guidProvider;

        public EventDataBuilder(IGuidProvider guidProvider)
        {
            _guidProvider = guidProvider;
        }

        public EventData BuildEventData<T>(IEvent eventData) where T : IAggregate
        {
            var id = _guidProvider.GenerateGuid();

            var properties = new EventStorageData(
                typeof(T).Name,
                id.ToString("D"),
                eventData.GetType().AssemblyQualifiedName,
                JsonConvert.SerializeObject(eventData));

            return new EventData(EventId.From(id), EventProperties.From(properties));
        }
    }
}