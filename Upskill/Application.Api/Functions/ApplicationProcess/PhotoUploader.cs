using System.Threading.Tasks;
using Application.Api.Events.Internal;
using Application.Commands.Commands;
using Application.Storage.Blobs.Providers;
using Application.Storage.Blobs.Writers;
using Application.Storage.Constants;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Upskill.FunctionUtils.Extensions;

namespace Application.Api.Functions.ApplicationProcess
{
    public class PhotoUploader
    {
        private readonly IFileWriter _fileWriter;
        private readonly IFileNameProvider _fileNameProvider;

        public PhotoUploader(
            IFileWriter fileWriter,
            IFileNameProvider fileNameProvider)
        {
            _fileWriter = fileWriter;
            _fileNameProvider = fileNameProvider;
        }

        [FunctionName(nameof(PhotoUploader))]
        public async Task Run(
            [DurableClient] IDurableOrchestrationClient client,
            [ActivityTrigger] IDurableActivityContext context,
            ExecutionContext executionContext,
            ILogger log)
        {
            executionContext.CorrelateExecution(context.InstanceId);
            var command = context.GetInput<UploadPhotoCommand>();

            var photoSaveResult = await _fileWriter.Write(
                FileStore.PhotosContainer,
                command.Content,
                command.ContentType,
                _fileNameProvider.GetFileName(context.InstanceId, command.Extension));

            if (!photoSaveResult.Success)
            {
                log.LogError($"Uploading photo failed instanceId: {context.InstanceId}", photoSaveResult.Errors, context.InstanceId);
                var failedEvent = new CvUploadFailedInternalFunctionEvent(photoSaveResult.Errors);
                await client.RaiseEventAsync(context.InstanceId, nameof(CvUploadFailedInternalFunctionEvent), failedEvent);
            }

            var eventToDispatch = new PhotoUploadedInternalFunctionEvent(photoSaveResult.Value);
            await client.RaiseEventAsync(context.InstanceId, nameof(PhotoUploadedInternalFunctionEvent), eventToDispatch);
        }
    }
}