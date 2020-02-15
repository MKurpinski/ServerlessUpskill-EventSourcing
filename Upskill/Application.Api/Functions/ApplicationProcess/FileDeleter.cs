using System.Threading.Tasks;
using Application.Commands.Commands;
using Application.Storage.Blobs.Deleters;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.Telemetry.CorrelationInitializers;

namespace Application.Api.Functions.ApplicationProcess
{
    public class FileDeleter
    {
        private readonly IFileDeleter _fileDeleter;
        private readonly ICorrelationInitializer _correlationInitializer;

        public FileDeleter(
            IFileDeleter fileDeleter,
            ICorrelationInitializer correlationInitializer)
        {
            _fileDeleter = fileDeleter;
            _correlationInitializer = correlationInitializer;
        }

        [FunctionName(nameof(FileDeleter))]
        public async Task Run(
            [ActivityTrigger] IDurableActivityContext context)
        {
            _correlationInitializer.Initialize(context.InstanceId);

            var command = context.GetInput<DeleteFileCommand>();
            await _fileDeleter.Delete(command.ContainerName, command.FileName);
        }
    }
}