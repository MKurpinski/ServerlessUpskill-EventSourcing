using Application.Storage.Providers;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Application.Storage.Blob.Providers
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
