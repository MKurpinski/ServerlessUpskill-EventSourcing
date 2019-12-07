namespace Application.BlobStorage.Providers
{
    public class FileNameProvider : IFileNameProvider
    {
        public string GetFileName(string fileName, string extension)
        {
            return $"{fileName}.{extension}";
        }
    }
}
