using System.Threading.Tasks;
using Application.Commands.Commands;
using Application.Storage.Blobs.Deleters;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.Logging.TelemetryInitialization;

namespace Application.Api.Functions.ApplicationProcess
{
    public class FileDeleter
    {
        private readonly IFileDeleter _fileDeleter;
        private readonly ITelemetryInitializer _telemetryInitializer;

        public FileDeleter(
            IFileDeleter fileDeleter,
            ITelemetryInitializer telemetryInitializer)
        {
            _fileDeleter = fileDeleter;
            _telemetryInitializer = telemetryInitializer;
        }

        [FunctionName(nameof(FileDeleter))]
        public async Task Run(
            [ActivityTrigger] IDurableActivityContext context)
        {
            _telemetryInitializer.Initialize(context.InstanceId);

            var command = context.GetInput<DeleteFileCommand>();
            await _fileDeleter.Delete(command.ContainerName, command.FileName);
        }
    }
}