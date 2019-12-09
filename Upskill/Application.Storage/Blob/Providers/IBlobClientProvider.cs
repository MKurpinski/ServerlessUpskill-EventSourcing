using Microsoft.WindowsAzure.Storage.Blob;

namespace Application.Storage.Blob.Providers
{
    public interface IBlobClientProvider
    {
        CloudBlobClient Get();
    }
}
