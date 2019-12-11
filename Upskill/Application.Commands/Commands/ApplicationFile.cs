namespace Application.Commands.Commands
{
    public class ApplicationFile
    {
        public byte[] File { get; }
        public string ContentType { get; }
        public string Extension { get; }

        public ApplicationFile(
            byte[] file, 
            string contentType,
            string extension)
        {
            File = file;
            ContentType = contentType;
            Extension = extension;
        }
    }
}
