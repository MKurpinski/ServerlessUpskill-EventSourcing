namespace Application.Storage.Blobs.Providers
{
    public interface IFileNameProvider
    {
        string GetFileName(string fileName, string extension);
    }
}
