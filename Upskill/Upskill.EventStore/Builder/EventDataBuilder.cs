﻿using Newtonsoft.Json;
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

        public EventData BuildEventData<T>(IEvent eventData) where T : IAggregateRoot
        {
            var id = _guidProvider.GenerateGuid();

            var properties = new EventStorageData(
                typeof(T).Name,
                id,
                eventData.GetType().AssemblyQualifiedName,
                JsonConvert.SerializeObject(eventData));

            return new EventData(EventId.From(id), EventProperties.From(properties));
        }
    }
}