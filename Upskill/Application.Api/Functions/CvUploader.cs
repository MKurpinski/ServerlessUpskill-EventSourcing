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
    public class CvUploader
    {
        private readonly IFileWriter _fileWriter;
        private readonly IFileNameProvider _fileNameProvider;

        public CvUploader(
            IFileWriter fileWriter,
            IFileNameProvider fileNameProvider)
        {
            _fileWriter = fileWriter;
            _fileNameProvider = fileNameProvider;
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
                    FileStore.CvsContainer,
                    command.Content,
                    command.ContentType,
                    _fileNameProvider.GetFileName(context.InstanceId, command.Extension));

                var eventToDispatch = new CvUploadedEvent(cvUri);
                await client.RaiseEventAsync(context.InstanceId, nameof(CvUploadedEvent), eventToDispatch);
            }
            catch (Exception ex)
            {
                log.LogError($"Uploading cv failed instanceId: {context.InstanceId}, error: {ex.Message}");
                var eventToDispatch = new CvUploadFailedEvent();
                await client.RaiseEventAsync(context.InstanceId, nameof(CvUploadFailedEvent), eventToDispatch);
            }
        }
    }
}