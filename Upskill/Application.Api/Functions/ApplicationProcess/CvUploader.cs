using System.Threading.Tasks;
using Application.Api.Events.Internal;
using Application.Commands.Commands;
using Application.Storage.Blobs.Providers;
using Application.Storage.Blobs.Writers;
using Application.Storage.Constants;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Upskill.Telemetry.CorrelationInitializers;

namespace Application.Api.Functions.ApplicationProcess
{
    public class CvUploader
    {
        private readonly IFileWriter _fileWriter;
        private readonly IFileNameProvider _fileNameProvider;
        private readonly ICorrelationInitializer _correlationInitializer;

        public CvUploader(
            IFileWriter fileWriter,
            IFileNameProvider fileNameProvider,
            ICorrelationInitializer correlationInitializer)
        {
            _fileWriter = fileWriter;
            _fileNameProvider = fileNameProvider;
            _correlationInitializer = correlationInitializer;
        }

        [FunctionName(nameof(CvUploader))]
        public async Task Run(
            [DurableClient] IDurableOrchestrationClient client,
            [ActivityTrigger] IDurableActivityContext context)
        {
            _correlationInitializer.Initialize(context.InstanceId);

            var command = context.GetInput<UploadCvCommand>();
            var saveCvResult = await _fileWriter.Write(
                FileStore.CvsContainer,
                command.Content,
                command.ContentType,
                _fileNameProvider.GetFileName(context.InstanceId, command.Extension));

            if (!saveCvResult.Success)
            {
                var failedEvent = new CvUploadFailedInternalFunctionEvent(saveCvResult.Errors);
                await client.RaiseEventAsync(context.InstanceId, nameof(CvUploadFailedInternalFunctionEvent), failedEvent);
            }

            var eventToDispatch = new CvUploadedInternalFunctionEvent(saveCvResult.Value);
            await client.RaiseEventAsync(context.InstanceId, nameof(CvUploadedInternalFunctionEvent), eventToDispatch);
        }
    }
}