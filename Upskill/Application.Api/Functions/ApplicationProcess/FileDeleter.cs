using System.Threading.Tasks;
using Application.Commands.Commands;
using Application.Storage.Blobs.Deleters;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Application.Api.Functions.ApplicationProcess
{
    public class FileDeleter
    {
        private readonly IFileDeleter _fileDeleter;

        public FileDeleter(IFileDeleter fileDeleter)
        {
            _fileDeleter = fileDeleter;
        }

        [FunctionName(nameof(FileDeleter))]
        public async Task Run(
            [ActivityTrigger] IDurableActivityContext context)
        {
            var command = context.GetInput<DeleteFileCommand>();
            await _fileDeleter.Delete(command.ContainerName, command.FileName);
        }
    }
}