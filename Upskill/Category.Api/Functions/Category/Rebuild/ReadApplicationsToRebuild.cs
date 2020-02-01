using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.EventStore.Providers;

namespace Category.Api.Functions.Category.Rebuild
{
    public class ReadCategoriesToRebuild
    {
        private readonly IStreamLogProvider<Core.Aggregates.Category> _streamLogProvider;

        public ReadCategoriesToRebuild(
            IStreamLogProvider<Core.Aggregates.Category> streamLogProvider)
        {
            _streamLogProvider = streamLogProvider;
        }

        [FunctionName(nameof(ReadCategoriesToRebuild))]
        public async Task<IReadOnlyCollection<string>> Run([ActivityTrigger] IDurableActivityContext context)
        {
            var application = await _streamLogProvider.GetStreams();
            return application;
        }
    }
}