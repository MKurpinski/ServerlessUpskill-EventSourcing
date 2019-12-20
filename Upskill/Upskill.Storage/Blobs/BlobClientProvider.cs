using Microsoft.WindowsAzure.Storage.Blob;
using Upskill.Storage.Providers;

namespace Upskill.Storage.Blobs
{
    public class BlobClientProvider: IBlobClientProvider
    {
        private readonly IStorageAccountProvider _storageAccountProvider;
        public BlobClientProvider(IStorageAccountProvider storageAccountProvider)
        {
            _storageAccountProvider = storageAccountProvider;
        }

        public CloudBlobClient Get()
        {
            var storageAccount = _storageAccountProvider.Get();

            return storageAccount.CreateCloudBlobClient();
        }
    }
}
