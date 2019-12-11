namespace Application.Commands.Commands
{
    public abstract class BaseUploadFileCommand : ICommand
    {
        public byte[] Content { get; }
        public string ContentType { get; }
        public string Extension { get; }

        protected BaseUploadFileCommand(byte[] content, string contentType, string extension)
        {
            Content = content;
            ContentType = contentType;
            Extension = extension;
        }
    }
}
