namespace Application.Commands.Commands
{
    public class DeleteFileCommand
    {
        public string FileName { get; }
        public string ContainerName { get; }

        public DeleteFileCommand(string fileName, string containerName)
        {
            FileName = fileName;
            ContainerName = containerName;
        }
    }
}
