namespace Application.BlobStorage.Providers
{
    public interface IFileNameProvider
    {
        string GetFileName(string fileName, string extension);
    }
}
