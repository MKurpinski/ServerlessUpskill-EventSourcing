using System;
using System.Threading.Tasks;
using Application.Api.Constants;
using Application.Api.Events.Internal;
using Application.BlobStorage.Providers;
using Application.BlobStorage.Writers;
using Application.Commands.Commands;
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
            try
            {
                var command = context.GetInput<UploadPhotoCommand>();

                var cvUri = await _fileWriter.Write(
                    FileStore.PhotosContainer,
                    command.Content,
                    command.ContentType,
                    _fileNameProvider.GetFileName(context.InstanceId, command.Extension));

                var eventToDispatch = new PhotoUploadedEvent(cvUri);
                await client.RaiseEventAsync(context.InstanceId, nameof(PhotoUploadedEvent), eventToDispatch);
            }
            catch (Exception ex)
            {
                log.LogError($"Uploading photo failed instanceId: {context.InstanceId}, error: {ex.Message}");
                var eventToDispatch = new CvUploadFailedEvent();
                await client.RaiseEventAsync(context.InstanceId, nameof(CvUploadFailedEvent), eventToDispatch);
            }
        }
    }
}