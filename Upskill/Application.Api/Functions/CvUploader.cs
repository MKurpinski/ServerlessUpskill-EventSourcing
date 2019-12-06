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
    public class CvUploader
    {
        private const string CV_CONTAINER_NAME = "cv-store";
        private readonly IFileWriter _fileWriter;

        public CvUploader(IFileWriter fileWriter)
        {
            _fileWriter = fileWriter;
        }

        [FunctionName(nameof(CvUploader))]
        public async Task Run(
            [DurableClient] IDurableOrchestrationClient client,
            [ActivityTrigger] IDurableActivityContext context,
            ILogger log)
        {
            try
            {
                var command = context.GetInput<UploadCvCommand>();
                var cvUri = await _fileWriter.Write(
                    CV_CONTAINER_NAME,
                    command.Content,
                    command.ContentType,
                    context.InstanceId,
                    command.Extension);

                var eventToDispatch = new CvUploadedEvent(cvUri);
                await client.RaiseEventAsync(context.InstanceId, nameof(CvUploadedEvent), eventToDispatch);
            }
            catch (Exception ex)
            {
                log.LogError($"Uploading cv failed instanceId: {context.InstanceId}, error: {ex.Message}");
                var eventToDispatch = new CvUploadFailed();
                await client.RaiseEventAsync(context.InstanceId, nameof(CvUploadFailed), eventToDispatch);
            }
        }
    }
}