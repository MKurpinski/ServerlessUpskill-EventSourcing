// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName=ApplicationAddedEventHandler

using Application.Api.Events.External.ApplicationAdded;
using Application.Search.Indexer;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Api.Functions.Search
{
    public class ApplicationAddedEventHandler
    {
        private readonly IApplicationIndexer _applicationIndexer;

        public ApplicationAddedEventHandler(IApplicationIndexer applicationIndexer)
        {
            _applicationIndexer = applicationIndexer;
        }

        [FunctionName(nameof(ApplicationAddedEventHandler))]
        public void Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            var applicationAddedEvent = JsonConvert.DeserializeObject<ApplicationAddedEvent>(eventGridEvent.Data.ToString());
            log.LogInformation($"{nameof(ApplicationAddedEvent)} with id: {applicationAddedEvent.Id}, indexing started");
            _applicationIndexer.Index(applicationAddedEvent);
            log.LogInformation($"{nameof(ApplicationAddedEvent)} with id: {applicationAddedEvent.Id}, indexing finished");
        }
    }
}
