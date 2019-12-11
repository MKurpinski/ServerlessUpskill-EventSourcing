using System.Threading.Tasks;
using Application.Commands.Commands;
using Application.ProcessStatus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Application.Api.Functions
{
    public class StatusTracker
    {
        private readonly IProcessStatusHandler _processStatusHandler;

        public StatusTracker(IProcessStatusHandler processStatusHandler)
        {
            _processStatusHandler = processStatusHandler;
        }

        [FunctionName(nameof(StatusTracker))]
        public async Task Run(
            [ActivityTrigger] IDurableActivityContext context)
        {
            var command = context.GetInput<TrackProcessStatusCommand>();
            await _processStatusHandler.TrackStatus(
                command.CorrelationId,
                command.Status,
                command.Errors);
        }
    }
}