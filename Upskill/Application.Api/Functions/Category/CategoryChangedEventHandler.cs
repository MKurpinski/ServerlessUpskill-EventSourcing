// http://localhost:7071/runtime/webhooks/EventGrid?functionName=CategoryChangedEventHandler

using System.Threading.Tasks;
using Application.Api.Events.External;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Api.Functions.Category
{
    public class CategoryChangedEventHandler
    {
        [FunctionName(nameof(CategoryChangedEventHandler))]
        public async Task Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            var categoryChangedEvent = JsonConvert.DeserializeObject<CategoryChangedEvent>(eventGridEvent.Data.ToString());
            log.LogInformation($"{nameof(CategoryChangedEventHandler)}: starting updating applications from category: {categoryChangedEvent.Id}");
            
            log.LogInformation($"{nameof(CategoryChangedEventHandler)}: finished updating applications from category: {categoryChangedEvent.Id}");
        }
    }
}
