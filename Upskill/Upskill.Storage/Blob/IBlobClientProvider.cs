using Microsoft.WindowsAzure.Storage.Blob;

namespace Upskill.Storage.Blob
{
    public interface IBlobClientProvider
    {
        CloudBlobClient Get();
    }
}
