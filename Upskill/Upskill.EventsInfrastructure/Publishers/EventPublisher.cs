using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Upskill.Events;
using Upskill.EventsInfrastructure.Clients;
using Upskill.EventsInfrastructure.Options;
using Upskill.Infrastructure;

namespace Upskill.EventsInfrastructure.Publishers
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IGuidProvider _guidProvider;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly EventOptions _eventOptions;
        private readonly Lazy<IDictionary<string, EventInformation>> _lazyTopicNameInformationMap;
        private readonly ILogger<EventPublisher> _logger;
        private readonly IEventGridClientFacade _eventGridClientFacade;

        public EventPublisher(
            ILogger<EventPublisher> logger,
            IGuidProvider guidProvider,
            IDateTimeProvider dateTimeProvider,
            IOptions<EventOptions> eventOptionsAccessor,
            IEventGridClientFacade eventGridClientFacade)
        {
            _logger = logger;
            _guidProvider = guidProvider;
            _dateTimeProvider = dateTimeProvider;
            _eventGridClientFacade = eventGridClientFacade;
            _eventOptions = eventOptionsAccessor.Value;
            _lazyTopicNameInformationMap = new Lazy<IDictionary<string, EventInformation>>(GetTopicsMap);
        }

        public async Task PublishEvent<T>(T eventContent) where T : IEvent
        {
            var typeName = eventContent.GetType().Name;

            var canHandle = _lazyTopicNameInformationMap.Value.TryGetValue(typeName, out var eventInformation);

            if (!canHandle)
            {
                _logger.LogInformation($"Event of type: {typeName} cannot be handled");
                return;
            }

            var domainHostname = new Uri(_eventOptions.DomainEndpoint).Host;

            var domainCredentials = new TopicCredentials(_eventOptions.DomainKey);

            var serializedEventContent = JsonConvert.SerializeObject(eventContent, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            var eventToPublish = new EventGridEvent
            {
                Id = _guidProvider.GenerateGuid(),
                Data = serializedEventContent,
                DataVersion = eventInformation.EventVersion,
                EventTime = _dateTimeProvider.GetCurrentDateTime(),
                EventType = typeName,
                Topic = typeName,
                Subject = typeName
            };

            await _eventGridClientFacade.PublishEvent(domainCredentials, domainHostname, eventToPublish);
        }

        private IDictionary<string, EventInformation> GetTopicsMap()
        {
            return _eventOptions.Events.ToDictionary(t => t.EventName, t => t);
        }
    }
}
