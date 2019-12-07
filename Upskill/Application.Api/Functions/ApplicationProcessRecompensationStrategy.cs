using System.Threading.Tasks;
using Application.Api.Constants;
using Application.BlobStorage.Providers;
using Application.Commands.Commands;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Application.Api.Functions
{
    public class ApplicationProcessRecompensationOrchiestrator
    {
        private readonly IFileNameProvider _fileNameProvider;

        public ApplicationProcessRecompensationOrchiestrator(IFileNameProvider fileNameProvider)
        {
            _fileNameProvider = fileNameProvider;
        }

        [FunctionName(nameof(ApplicationProcessRecompensationOrchiestrator))]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            var command = context.GetInput<RecompensateApplicationProcessCommand>();
            log.LogInformation($"Starting recompensation process(instanceId: {context.InstanceId} for ${command.Id}");
            
            var deleteCvCommand = new DeleteFileCommand(
                _fileNameProvider.GetFileName(command.Id, command.Cv.Extension),
                FileStore.CvsContainer);

            var deletePhotoCommand = new DeleteFileCommand(
                _fileNameProvider.GetFileName(command.Id, command.Photo.Extension),
                FileStore.PhotosContainer);

            await context.CallActivityAsync(nameof(FileDeleter), deleteCvCommand);
            await context.CallActivityAsync(nameof(FileDeleter), deletePhotoCommand);
            log.LogInformation($"Finished recompensation process(instanceId: {context.InstanceId} for ${command.Id}");
        }
    }
}