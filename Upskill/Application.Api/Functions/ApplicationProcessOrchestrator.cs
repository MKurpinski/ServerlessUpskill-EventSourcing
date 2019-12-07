using System.Threading.Tasks;
using Application.Api.Events.Internal;
using Application.Commands.Commands;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Application.Api.Functions
{
    public class ApplicationProcessOrchestrator
    {
        [FunctionName(nameof(ApplicationProcessOrchestrator))]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            [DurableClient] IDurableOrchestrationClient processStarter,
            ILogger log)
        {
            log.LogInformation($"Starting orchestration of application process with instance id: {context.InstanceId}");
            
            var command = context.GetInput<RegisterApplicationCommand>();

            var uploadCvCommand = new UploadCvCommand(
                command.Cv.File,
                command.Cv.ContentType,
                command.Cv.Extension);

            var uploadPhotoCommand = new UploadPhotoCommand(
                command.Photo.File,
                command.Photo.ContentType,
                command.Photo.Extension);

            await Task.WhenAll(
                context.CallActivityAsync<Task>(nameof(CvUploader), uploadCvCommand),
                context.CallActivityAsync<Task>(nameof(PhotoUploader), uploadPhotoCommand));

            var cvUploadedEventTask = context.WaitForExternalEvent<CvUploadedEvent>(nameof(CvUploadedEvent));
            var cvUploadFailedEventTask = context.WaitForExternalEvent<CvUploadFailedEvent>(nameof(CvUploadFailedEvent));

            var photoUploadedEventTask = context.WaitForExternalEvent<PhotoUploadedEvent>(nameof(PhotoUploadedEvent));
            var photoUploadFailedEventTask = context.WaitForExternalEvent<PhotoUploadFailedEvent>(nameof(PhotoUploadFailedEvent));

            var cvUploadEventTask = await Task.WhenAny(cvUploadedEventTask, cvUploadFailedEventTask);
            var photoUploadEventTask = await Task.WhenAny(photoUploadedEventTask, photoUploadFailedEventTask);

            var cvUploadedSuccessfully = cvUploadEventTask == cvUploadedEventTask;
            var photoUploadedSuccessfully = photoUploadEventTask == photoUploadedEventTask;

            if (!cvUploadedSuccessfully || !photoUploadedSuccessfully)
            {
                log.LogError($"Uploading files failed: {context.InstanceId}");
                await this.StartRecompensateProcess(processStarter, context, command, log);
                return;
            }

            log.LogInformation($"Finished the files uploading in instanceId: {context.InstanceId}");

            var cvUri = cvUploadedEventTask.Result.CvUri;
            var photoUri = photoUploadedEventTask.Result.PhotoUri;

            var saveApplicationCommand = new SaveApplicationCommand(
                command.Candidate.FirstName,
                command.Candidate.LastName,
                photoUri,
                cvUri,
                command.Candidate.Category,
                command.CreationTime);

            await context.CallActivityAsync<Task>(nameof(ApplicationSaver), saveApplicationCommand);

            var applicationSavedEvent = context.WaitForExternalEvent<ApplicationSavedEvent>(nameof(ApplicationSavedEvent));
            var applicationSaveFailed = context.WaitForExternalEvent<ApplicationSaveFailedEvent>(nameof(ApplicationSaveFailedEvent));
            var applicationSaveEvent = await Task.WhenAny(applicationSavedEvent, applicationSaveFailed);

            
            var applicationSavedSuccessfully = applicationSaveEvent == applicationSavedEvent;
            if (!applicationSavedSuccessfully)
            {
                log.LogError($"Storing application failed with instance id: {context.InstanceId}");
                await this.StartRecompensateProcess(processStarter, context, command, log);
                return;
            }

            log.LogInformation($"Application process finished: {context.InstanceId}");

            //ToDo dispatch events to event bus
        }

        private async Task StartRecompensateProcess(
            IDurableOrchestrationClient processStarter,
            IDurableOrchestrationContext context,
            RegisterApplicationCommand command,
            ILogger log)
        {
            var recompensateCommand = this.BuildRecompensationCommand(context, command);
            var recompensationId = await processStarter.StartNewAsync(nameof(ApplicationProcessRecompensationOrchiestrator), recompensateCommand);
            log.LogInformation($"Started recompensation process for application process with instanceId: {context.InstanceId}." +
                               $"Recompensation process instanceId: {recompensationId}");
        }

        private RecompensateApplicationProcessCommand BuildRecompensationCommand(
            IDurableOrchestrationContext context,
            RegisterApplicationCommand command)
        {
            return new RecompensateApplicationProcessCommand(context.InstanceId, command.Photo, command.Cv);
        }
    }
}