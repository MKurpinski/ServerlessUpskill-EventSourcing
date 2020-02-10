using System.Threading.Tasks;
using Application.Commands.Commands;
using Application.ProcessStatus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.FunctionUtils.Extensions;

namespace Application.Api.Functions.ApplicationProcess
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
            [ActivityTrigger] IDurableActivityContext context,
            ExecutionContext executionContext)
        {
            var command = context.GetInput<TrackProcessStatusCommand>();
            executionContext.CorrelateExecution(command.CorrelationId);
            await _processStatusHandler.TrackStatus(
                command.CorrelationId,
                command.Status,
                command.Errors);
        }
    }
}