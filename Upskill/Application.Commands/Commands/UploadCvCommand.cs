namespace Application.Commands.Commands
{
    public class UploadCvCommand : BaseUploadFileCommand
    {
        public UploadCvCommand(byte[] content, string contentType, string extension) 
            : base(content, contentType, extension)
        {
        }
    }
}
