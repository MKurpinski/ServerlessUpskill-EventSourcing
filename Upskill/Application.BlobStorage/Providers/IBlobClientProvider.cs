using Microsoft.WindowsAzure.Storage.Blob;

namespace Application.BlobStorage.Providers
{
    public interface IBlobClientProvider
    {
        CloudBlobClient Get();
    }
}
