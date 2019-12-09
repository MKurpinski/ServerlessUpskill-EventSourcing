namespace Application.Storage.Blob.Providers
{
    public interface IFileNameProvider
    {
        string GetFileName(string fileName, string extension);
    }
}
