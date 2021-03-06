using System;
using System.Threading.Tasks;
using Application.Commands.Commands;
using Application.Storage.Blobs.Providers;
using Application.Storage.Constants;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Upskill.Telemetry.CorrelationInitializers;

namespace Application.Api.Functions.ApplicationProcess
{
    public class ApplicationProcessRecompensationOrchestrator
    {
        private readonly IFileNameProvider _fileNameProvider;
        private readonly ICorrelationInitializer _correlationInitializer;

        public ApplicationProcessRecompensationOrchestrator(
            IFileNameProvider fileNameProvider,
            ICorrelationInitializer correlationInitializer)
        {
            _fileNameProvider = fileNameProvider;
            _correlationInitializer = correlationInitializer;
        }

        [FunctionName(nameof(ApplicationProcessRecompensationOrchestrator))]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            _correlationInitializer.Initialize(context.InstanceId);

            try
            {
                var command = context.GetInput<RecompensateApplicationProcessCommand>();
                log.LogInformation(
                    $"Starting recompensation process(instanceId: {context.InstanceId} for ${command.Id}");

                var deleteCvCommand = new DeleteFileCommand(
                    _fileNameProvider.GetFileName(command.Id, command.Cv.Extension),
                    FileStore.CvsContainer);

                var deletePhotoCommand = new DeleteFileCommand(
                    _fileNameProvider.GetFileName(command.Id, command.Photo.Extension),
                    FileStore.PhotosContainer);

                await context.CallActivityAsync(nameof(FileDeleter), deleteCvCommand);
                await context.CallActivityAsync(nameof(FileDeleter), deletePhotoCommand);
                log.LogInformation(
                    $"Finished recompensation process(instanceId: {context.InstanceId} for ${command.Id}");
            }
            catch (Exception)
            {
                // Allow somebody from support to handle it manually
                throw;
            }
        }
    }
}