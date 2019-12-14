// http://localhost:7071/runtime/webhooks/EventGrid?functionName=ApplicationAddedEventHandler

using System.Threading.Tasks;
using Application.Api.Events.External.ApplicationAdded;
using Application.Search.Dtos;
using Application.Search.Indexer;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Api.Functions.Search
{
    public class ApplicationAddedEventHandler
    {
        private readonly ISearchableApplicationIndexer _searchableApplicationIndexer;
        private readonly IMapper _mapper;

        public ApplicationAddedEventHandler(
            ISearchableApplicationIndexer searchableApplicationIndexer,
            IMapper mapper)
        {
            _searchableApplicationIndexer = searchableApplicationIndexer;
            _mapper = mapper;
        }

        [FunctionName(nameof(ApplicationAddedEventHandler))]
        public async Task Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            var applicationAddedEvent = JsonConvert.DeserializeObject<ApplicationAddedEvent>(eventGridEvent.Data.ToString());
            log.LogInformation($"{nameof(ApplicationAddedEvent)} with id: {applicationAddedEvent.Id}, indexing started");
            
            var applicationDto = _mapper.Map<ApplicationAddedEvent, ApplicationDto>(applicationAddedEvent);
            await _searchableApplicationIndexer.Index(applicationDto);

            log.LogInformation($"{nameof(ApplicationAddedEvent)} with id: {applicationAddedEvent.Id}, indexing finished");
        }
    }
}
