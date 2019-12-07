using System.Threading.Tasks;
using Application.BlobStorage.Deleters;
using Application.Commands.Commands;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace Application.Api.Functions
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