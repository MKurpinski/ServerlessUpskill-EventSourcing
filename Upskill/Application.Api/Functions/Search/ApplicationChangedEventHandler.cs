// http://localhost:7071/runtime/webhooks/EventGrid?functionName=ApplicationChangedEventHandler

using System.Threading.Tasks;
using Application.Api.Events.External.ApplicationChanged;
using Application.Search.Dtos;
using Application.Search.Indexers;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Api.Functions.Search
{
    public class ApplicationChangedEventHandler
    {
        private readonly ISearchableApplicationIndexer _searchableApplicationIndexer;
        private readonly IMapper _mapper;

        public ApplicationChangedEventHandler(
            ISearchableApplicationIndexer searchableApplicationIndexer,
            IMapper mapper)
        {
            _searchableApplicationIndexer = searchableApplicationIndexer;
            _mapper = mapper;
        }

        [FunctionName(nameof(ApplicationChangedEventHandler))]
        public async Task Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            var applicationAddedEvent = JsonConvert.DeserializeObject<ApplicationChangedEvent>(eventGridEvent.Data.ToString());
            log.LogInformation($"{nameof(ApplicationChangedEvent)} with id: {applicationAddedEvent.Id}, indexing started");
            
            var applicationDto = _mapper.Map<ApplicationChangedEvent, ApplicationDto>(applicationAddedEvent);
            await _searchableApplicationIndexer.Index(applicationDto);

            log.LogInformation($"{nameof(ApplicationChangedEvent)} with id: {applicationAddedEvent.Id}, indexing finished");
        }
    }
}
