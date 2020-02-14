using System.Threading.Tasks;
using Application.Commands.Commands;
using Application.ProcessStatus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.Logging.TelemetryInitialization;

namespace Application.Api.Functions.ApplicationProcess
{
    public class StatusTracker
    {
        private readonly IProcessStatusHandler _processStatusHandler;
        private readonly ITelemetryInitializer _telemetryInitializer;

        public StatusTracker(
            IProcessStatusHandler processStatusHandler,
            ITelemetryInitializer telemetryInitializer)
        {
            _processStatusHandler = processStatusHandler;
            _telemetryInitializer = telemetryInitializer;
        }

        [FunctionName(nameof(StatusTracker))]
        public async Task Run(
            [ActivityTrigger] IDurableActivityContext context)
        {
            var command = context.GetInput<TrackProcessStatusCommand>();
            _telemetryInitializer.Initialize(command.CorrelationId);
            await _processStatusHandler.TrackStatus(
                command.CorrelationId,
                command.Status,
                command.Errors);
        }
    }
}