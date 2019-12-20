using Microsoft.WindowsAzure.Storage.Blob;

namespace Upskill.Storage.Blobs
{
    public interface IBlobClientProvider
    {
        CloudBlobClient Get();
    }
}
