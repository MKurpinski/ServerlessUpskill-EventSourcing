using System.Threading.Tasks;
using Application.Core.Events.ApplicationChangedEvent;
using Application.Search.Dtos;
using Application.Search.Indexers;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Upskill.Events;

namespace Application.Core.EventHandlers
{
    public class ApplicationChangedEventHandler : IEventHandler<ApplicationChangedEvent>
    {
        private readonly ISearchableApplicationIndexer _searchableApplicationIndexer;
        private readonly IMapper _mapper;
        private readonly ILogger<ApplicationChangedEventHandler> _logger;

        public ApplicationChangedEventHandler(
            ISearchableApplicationIndexer searchableApplicationIndexer,
            IMapper mapper,
            ILogger<ApplicationChangedEventHandler> logger)
        {
            _searchableApplicationIndexer = searchableApplicationIndexer;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Handle(ApplicationChangedEvent applicationChangedEvent)
        {
            _logger.LogInformation($"{nameof(ApplicationChangedEvent)} with id: {applicationChangedEvent.Id}, indexing started");

            var applicationDto = _mapper.Map<ApplicationChangedEvent, ApplicationDto>(applicationChangedEvent);
            await _searchableApplicationIndexer.Index(applicationDto);

            _logger.LogInformation($"{nameof(ApplicationChangedEvent)} with id: {applicationChangedEvent.Id}, indexing finished");
        }
    }
}
