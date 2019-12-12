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
        private readonly Lazy<IDictionary<string, TopicInformation>> _lazyTopicNameInformationMap;
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
            _lazyTopicNameInformationMap = new Lazy<IDictionary<string, TopicInformation>>(GetTopicsMap);
        }

        public async Task PublishEvent<T>(T eventContent) where T : IEvent
        {
            var typeName = typeof(T).Name;

            var canHandle = _lazyTopicNameInformationMap.Value.TryGetValue(typeName, out var topicInformation);

            if (!canHandle)
            {
                _logger.LogInformation($"Event of type: {typeof(T).Name} cannot be handled");
                return;
            }

            var topicEndpoint = string.Format(_eventOptions.TopicEndpointPattern, topicInformation.TopicName, topicInformation.RegionName);
            var topicHostname = new Uri(topicEndpoint).Host;

            var topicCredentials = new TopicCredentials(topicInformation.TopicKey);

            var eventToPublish = new EventGridEvent
            {
                Id = _guidProvider.GenerateGuid().ToString(),
                Data = eventContent,
                DataVersion = topicInformation.EventVersion,
                EventTime = _dateTimeProvider.GetCurrentDateTime(),
                EventType = typeName,
                Subject = typeName
            };
            await _eventGridClientFacade.PublishEvent(topicCredentials, topicHostname, eventToPublish);
        }

        private IDictionary<string, TopicInformation> GetTopicsMap()
        {
            return _eventOptions.Topics.ToDictionary(t => t.TopicName, t => t);
        }
    }
}
