using System.Collections.Generic;
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

            var cvUploadedEventTask = context.WaitForExternalEvent<CvUploadedEvent>(nameof(CvUploadedEvent));
            var photoUploadedEventTask = context.WaitForExternalEvent<PhotoUploadedEvent>(nameof(PhotoUploadedEvent));
            
            var tasks = new List<Task>
            {
                context.CallActivityAsync<Task>(nameof(CvUploader), uploadCvCommand),
                context.CallActivityAsync<Task>(nameof(PhotoUploader), uploadPhotoCommand),
                cvUploadedEventTask,
                photoUploadedEventTask
            };

            await Task.WhenAll(tasks);

            log.LogInformation($": {context.InstanceId}");

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
            var applicationSavedEvent = await context.WaitForExternalEvent<ApplicationSavedEvent>(nameof(ApplicationSavedEvent));

            //ToDo dispatch events to event bus
        }
    }
}