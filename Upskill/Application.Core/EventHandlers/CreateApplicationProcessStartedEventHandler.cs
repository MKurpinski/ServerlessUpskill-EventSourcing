using System.Threading.Tasks;
using Application.Core.Events;
using Application.Core.Events.CreateApplicationProcessStarted;
using Application.Search.Dtos;
using Application.Search.Indexers;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Upskill.Events;
using Upskill.EventsInfrastructure.Publishers;
using Upskill.EventStore;
using Upskill.Infrastructure.Enums;
using Upskill.Infrastructure.Extensions;

namespace Application.Core.EventHandlers
{
    public class CreateApplicationProcessStartedEventHandler : IEventHandler<CreateApplicationProcessStartedEvent>
    {
        private readonly ISearchableApplicationIndexer _searchableApplicationIndexer;
        private readonly IMapper _mapper;
        private readonly IEventStore<Aggregates.Application> _eventStore;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<CreateApplicationProcessStartedEventHandler> _logger;

        public CreateApplicationProcessStartedEventHandler(
            ISearchableApplicationIndexer searchableApplicationIndexer,
            IMapper mapper,
            ILogger<CreateApplicationProcessStartedEventHandler> logger,
            IEventStore<Aggregates.Application> eventStore, 
            IEventPublisher eventPublisher)
        {
            _searchableApplicationIndexer = searchableApplicationIndexer;
            _mapper = mapper;
            _logger = logger;
            _eventStore = eventStore;
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(CreateApplicationProcessStartedEvent createApplicationProcessStartedEvent)
        {
            _logger.LogProgress(OperationPhase.InProgress, "Indexing started", createApplicationProcessStartedEvent.CorrelationId);

            var applicationDto = _mapper.Map<CreateApplicationProcessStartedEvent, ApplicationDto>(createApplicationProcessStartedEvent);
            await _searchableApplicationIndexer.Index(applicationDto);

            var applicationCreatedEvent = new ApplicationCreatedEvent(createApplicationProcessStartedEvent);

            await _eventStore.AppendEvent(applicationCreatedEvent.Id, applicationCreatedEvent);
            await _eventPublisher.PublishEvent(applicationCreatedEvent);

            _logger.LogProgress(OperationPhase.Finished, string.Empty, createApplicationProcessStartedEvent.CorrelationId);
        }
    }
}
