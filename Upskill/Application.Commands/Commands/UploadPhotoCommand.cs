namespace Application.Commands.Commands
{
    public class UploadPhotoCommand : BaseUploadFileCommand
    {
        public UploadPhotoCommand(byte[] content, string contentType, string extension) : base(content, contentType, extension)
        {
        }
    }
}
