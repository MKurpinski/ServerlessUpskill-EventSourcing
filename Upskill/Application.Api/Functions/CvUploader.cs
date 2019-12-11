using System;
using System.Threading.Tasks;
using Application.Api.Constants;
using Application.Api.Events.Internal;
using Application.Api.Extensions;
using Application.Commands.Commands;
using Application.Storage.Blob.Providers;
using Application.Storage.Blob.Writers;
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
            var command = context.GetInput<UploadCvCommand>();
            var saveCvResult = await _fileWriter.Write(
                FileStore.CvsContainer,
                command.Content,
                command.ContentType,
                _fileNameProvider.GetFileName(context.InstanceId, command.Extension));

            if (!saveCvResult.Success)
            {
                log.LogErrors($"Uploading cv failed instanceId: {context.InstanceId}", saveCvResult.Errors);
                var failedEvent = new CvUploadFailedEvent(saveCvResult.Errors);
                await client.RaiseEventAsync(context.InstanceId, nameof(CvUploadFailedEvent), failedEvent);
            }

            var eventToDispatch = new CvUploadedEvent(saveCvResult.Value);
            await client.RaiseEventAsync(context.InstanceId, nameof(CvUploadedEvent), eventToDispatch);
        }
    }
}