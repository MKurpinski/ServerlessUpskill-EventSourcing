using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Api.Events.Internal;
using Application.Commands.Commands;
using Application.ProcessStatus.Enums;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Upskill.Infrastructure.Enums;
using Upskill.Infrastructure.Extensions;
using Upskill.Logging.TelemetryInitialization;

namespace Application.Api.Functions.ApplicationProcess
{
    public class ApplicationProcessOrchestrator
    {
        private readonly ITelemetryInitializer _telemetryInitializer;

        public ApplicationProcessOrchestrator(ITelemetryInitializer telemetryInitializer)
        {
            _telemetryInitializer = telemetryInitializer;
        }

        [FunctionName(nameof(ApplicationProcessOrchestrator))]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            [DurableClient] IDurableOrchestrationClient processStarter,
            ILogger log)
        {
            _telemetryInitializer.Initialize(context.InstanceId);

            var beginProcessCommand = this.BuildBeginProcessCommand(context);
            await context.CallActivityAsync<Task>(nameof(StatusTracker), beginProcessCommand);

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

            var cvUploadedEventTask = context.WaitForExternalEvent<CvUploadedInternalFunctionEvent>(nameof(CvUploadedInternalFunctionEvent));
            var cvUploadFailedEventTask = context.WaitForExternalEvent<CvUploadFailedInternalFunctionEvent>(nameof(CvUploadFailedInternalFunctionEvent));

            var photoUploadedEventTask = context.WaitForExternalEvent<PhotoUploadedInternalFunctionEvent>(nameof(PhotoUploadedInternalFunctionEvent));
            var photoUploadFailedEventTask = context.WaitForExternalEvent<PhotoUploadFailedInternalFunctionEvent>(nameof(PhotoUploadFailedInternalFunctionEvent));

            var cvUploadEventTask = await Task.WhenAny(cvUploadedEventTask, cvUploadFailedEventTask);
            var photoUploadEventTask = await Task.WhenAny(photoUploadedEventTask, photoUploadFailedEventTask);

            var cvUploadedSuccessfully = cvUploadEventTask == cvUploadedEventTask;
            var photoUploadedSuccessfully = photoUploadEventTask == photoUploadedEventTask;

            if (!cvUploadedSuccessfully || !photoUploadedSuccessfully)
            {
                await this.HandleUploadFilesFailure(
                    context,
                    processStarter,
                    log, 
                    photoUploadFailedEventTask, 
                    cvUploadFailedEventTask,
                    command);

                return;
            }

            log.LogProgress(OperationPhase.InProgress, "Finished the files uploading", context.InstanceId);

            var cvUri = cvUploadedEventTask.Result.CvUri;
            var photoUri = photoUploadedEventTask.Result.PhotoUri;

            var saveApplicationCommand = new CreateApplicationCommand(
                context.InstanceId,
                command.Candidate.FirstName,
                command.Candidate.LastName,
                photoUri,
                cvUri,
                command.Candidate.Category,
                command.CreationTime,
                command.Candidate.EducationLevel,
                command.Candidate.Address,
                command.Candidate.FinishedSchools,
                command.Candidate.ConfirmedSkills,
                command.Candidate.WorkExperiences,
                context.InstanceId);

            await context.CallActivityAsync<Task>(nameof(ApplicationSaver), saveApplicationCommand);

            var applicationSavedEvent = context.WaitForExternalEvent<ApplicationSavedInternalFunctionEvent>(nameof(ApplicationSavedInternalFunctionEvent));
            var applicationSaveFailed = context.WaitForExternalEvent<ApplicationSaveFailedInternalFunctionEvent>(nameof(ApplicationSaveFailedInternalFunctionEvent));
            var applicationSaveEvent = await Task.WhenAny(applicationSavedEvent, applicationSaveFailed);

            var applicationSavedSuccessfully = applicationSaveEvent == applicationSavedEvent;
            if (!applicationSavedSuccessfully)
            {
                log.LogFailedOperation(OperationPhase.Failed, "Storing application failed", applicationSaveFailed.Result.Errors, context.InstanceId);
                var failedProcessCommand = this.BuildFailedProcessCommand(context, applicationSaveFailed.Result.Errors);
                await context.CallActivityAsync<Task>(nameof(StatusTracker), failedProcessCommand);
                await this.StartRecompensateProcess(processStarter, context, command, log);
                return;
            }

            var finishProcessCommand = this.BuildFinishedProcessCommand(context);
            await context.CallActivityAsync<Task>(nameof(StatusTracker), finishProcessCommand);
        }

        private async Task HandleUploadFilesFailure(IDurableOrchestrationContext context,
            IDurableOrchestrationClient processStarter, ILogger log, Task<PhotoUploadFailedInternalFunctionEvent> photoUploadFailedEventTask,
            Task<CvUploadFailedInternalFunctionEvent> cvUploadFailedEventTask, RegisterApplicationCommand command)
        {
            var errors = photoUploadFailedEventTask.IsCompleted
                ? photoUploadFailedEventTask.Result.Errors.ToList()
                : new List<KeyValuePair<string, string>>();

            if (cvUploadFailedEventTask.IsCompleted)
            {
                errors.AddRange(cvUploadFailedEventTask.Result.Errors);
            }

            var failedProcessCommand = this.BuildFailedProcessCommand(context, errors);
            await context.CallActivityAsync<Task>(nameof(StatusTracker), failedProcessCommand);

            log.LogFailedOperation(OperationPhase.Failed, "Uploading files failed:", errors, context.InstanceId);
            await this.StartRecompensateProcess(processStarter, context, command, log);
        }


        private async Task StartRecompensateProcess(
            IDurableOrchestrationClient processStarter,
            IDurableOrchestrationContext context,
            RegisterApplicationCommand command,
            ILogger log)
        {
            await context.CallActivityAsync(nameof(ApplicationProcessFailedEventPublisher), ApplicationProcessStatus.Failed.ToString());
            var recompensateCommand = this.BuildRecompensationCommand(context, command);
            await processStarter.StartNewAsync(nameof(ApplicationProcessRecompensationOrchestrator), context.InstanceId, recompensateCommand);
            log.LogInformation($"Started recompensation process for application process with instanceId: {context.InstanceId}.");
        }

        private RecompensateApplicationProcessCommand BuildRecompensationCommand(
            IDurableOrchestrationContext context,
            RegisterApplicationCommand command)
        {
            return new RecompensateApplicationProcessCommand(context.InstanceId, command.Photo, command.Cv);
        }
        private TrackProcessStatusCommand BuildBeginProcessCommand(IDurableOrchestrationContext context)
        {
            return new TrackProcessStatusCommand(
                context.InstanceId,
                ApplicationProcessStatus.UnderProcessing.ToString(), 
                Enumerable.Empty<KeyValuePair<string, string>>());
        }
        private TrackProcessStatusCommand BuildFinishedProcessCommand(IDurableOrchestrationContext context)
        {
            return new TrackProcessStatusCommand(
                context.InstanceId,
                ApplicationProcessStatus.Finished.ToString(),
                Enumerable.Empty<KeyValuePair<string, string>>());
        }
        private TrackProcessStatusCommand BuildFailedProcessCommand(IDurableOrchestrationContext context, IEnumerable<KeyValuePair<string, string>> errors)
        {
            return new TrackProcessStatusCommand(
                context.InstanceId,
                ApplicationProcessStatus.Failed.ToString(),
                errors);
        }
    }
}