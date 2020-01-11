using System.Threading.Tasks;
using Application.Core.Events.ApplicationAddedEvent;
using Application.Search.Dtos;
using Application.Search.Indexers;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Upskill.Events;

namespace Application.Core.EventHandlers
{
    public class ApplicationAddedEventHandler : IEventHandler<ApplicationAddedEvent>
    {
        private readonly ISearchableApplicationIndexer _searchableApplicationIndexer;
        private readonly IMapper _mapper;
        private readonly ILogger<ApplicationAddedEventHandler> _logger;

        public ApplicationAddedEventHandler(
            ISearchableApplicationIndexer searchableApplicationIndexer,
            IMapper mapper,
            ILogger<ApplicationAddedEventHandler> logger)
        {
            _searchableApplicationIndexer = searchableApplicationIndexer;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Handle(ApplicationAddedEvent applicationAddedEvent)
        {
            _logger.LogInformation($"{nameof(ApplicationAddedEvent)} with id: {applicationAddedEvent.Id}, indexing started");

            var applicationDto = _mapper.Map<ApplicationAddedEvent, ApplicationDto>(applicationAddedEvent);
            await _searchableApplicationIndexer.Index(applicationDto);

            _logger.LogInformation($"{nameof(ApplicationAddedEvent)} with id: {applicationAddedEvent.Id}, indexing finished");
        }
    }
}
