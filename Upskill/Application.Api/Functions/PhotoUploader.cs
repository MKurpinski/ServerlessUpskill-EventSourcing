using System.Threading.Tasks;
using Application.Api.Constants;
using Application.Api.Events.Internal;
using Application.Commands.Commands;
using Application.Storage.Blob.Providers;
using Application.Storage.Blob.Writers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Application.Api.Functions
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
            ILogger log)
        {
            var command = context.GetInput<UploadPhotoCommand>();

            var photoSaveResult = await _fileWriter.Write(
                FileStore.PhotosContainer,
                command.Content,
                command.ContentType,
                _fileNameProvider.GetFileName(context.InstanceId, command.Extension));

            if (!photoSaveResult.Success)
            {
                log.LogError($"Uploading photo failed instanceId: {context.InstanceId}", photoSaveResult.Errors);
                var failedEvent = new CvUploadFailedEvent(photoSaveResult.Errors);
                await client.RaiseEventAsync(context.InstanceId, nameof(CvUploadFailedEvent), failedEvent);
            }

            var eventToDispatch = new PhotoUploadedEvent(photoSaveResult.Value);
            await client.RaiseEventAsync(context.InstanceId, nameof(PhotoUploadedEvent), eventToDispatch);
        }
    }
}