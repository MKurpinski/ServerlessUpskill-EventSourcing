using System;
using System.Threading.Tasks;
using Application.Api.Events.Internal;
using Application.BlobStorage.Writers;
using Application.Commands.Commands;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Application.Api.Functions
{
    public class PhotoUploader
    {
        private const string PHOTO_CONTAINER_NAME = "photo-store";
        private readonly IFileWriter _fileWriter;

        public PhotoUploader(IFileWriter fileWriter)
        {
            _fileWriter = fileWriter;
        }

        [FunctionName(nameof(PhotoUploader))]
        public async Task Run(
            [DurableClient] IDurableOrchestrationClient client,
            [ActivityTrigger] IDurableActivityContext context,
            ILogger log)
        {
            try
            {
                var command = context.GetInput<UploadPhotoCommand>();

                var cvUri = await _fileWriter.Write(
                    PHOTO_CONTAINER_NAME,
                    command.Content,
                    command.ContentType,
                    context.InstanceId,
                    command.Extension);

                var eventToDispatch = new PhotoUploadedEvent(cvUri);
                await client.RaiseEventAsync(context.InstanceId, nameof(PhotoUploadedEvent), eventToDispatch);
            }
            catch (Exception ex)
            {
                log.LogError($"Uploading photo failed instanceId: {context.InstanceId}, error: {ex.Message}");
                var eventToDispatch = new CvUploadFailed();
                await client.RaiseEventAsync(context.InstanceId, nameof(CvUploadFailed), eventToDispatch);
            }
        }
    }
}