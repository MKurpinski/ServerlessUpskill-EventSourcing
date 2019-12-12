using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Upskill.EventPublisher.Client;
using Upskill.EventPublisher.Options;
using Upskill.Infrastructure;

namespace Upskill.EventPublisher.Publishers
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IGuidProvider _guidProvider;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly EventOptions _eventOptions;
        private readonly Lazy<IDictionary<string, EventInformation>> _lazyTopicNameInformationMap;
        private readonly ILogger _logger;
        private readonly IEventGridClientFacade _eventGridClientFacade;

        public EventPublisher(
            ILogger logger,
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
            var typeName = typeof(T).Name;

            var canHandle = _lazyTopicNameInformationMap.Value.TryGetValue(typeName, out var eventInformation);

            if (!canHandle)
            {
                _logger.LogInformation($"Event of type: {typeName} cannot be handled");
                return;
            }

            var domainEndpoint = string.Format(_eventOptions.DomainEndpointPattern, _eventOptions.DomainName, _eventOptions.RegionName);
            var domainHostname = new Uri(domainEndpoint).Host;

            var domainCredentials = new TopicCredentials(_eventOptions.DomainKey);

            var eventToPublish = new EventGridEvent
            {
                Id = _guidProvider.GenerateGuid().ToString(),
                Data = eventContent,
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
            return _eventOptions.Topics.ToDictionary(t => t.EventName, t => t);
        }
    }
}
