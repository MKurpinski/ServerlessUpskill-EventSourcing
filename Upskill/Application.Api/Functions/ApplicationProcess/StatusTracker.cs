using System.Threading.Tasks;
using Application.Commands.Commands;
using Application.ProcessStatus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.Telemetry.CorrelationInitializers;

namespace Application.Api.Functions.ApplicationProcess
{
    public class StatusTracker
    {
        private readonly IProcessStatusHandler _processStatusHandler;
        private readonly ICorrelationInitializer _correlationInitializer;

        public StatusTracker(
            IProcessStatusHandler processStatusHandler,
            ICorrelationInitializer correlationInitializer)
        {
            _processStatusHandler = processStatusHandler;
            _correlationInitializer = correlationInitializer;
        }

        [FunctionName(nameof(StatusTracker))]
        public async Task Run(
            [ActivityTrigger] IDurableActivityContext context)
        {
            var command = context.GetInput<TrackProcessStatusCommand>();
            _correlationInitializer.Initialize(command.CorrelationId);
            await _processStatusHandler.TrackStatus(
                command.CorrelationId,
                command.Status,
                command.Errors);
        }
    }
}