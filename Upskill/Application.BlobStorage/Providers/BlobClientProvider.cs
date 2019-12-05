using Application.BlobStorage.Options;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Application.BlobStorage.Providers
{
    public class BlobClientProvider: IBlobClientProvider
    {
        private readonly BlobStorageOptions _options;
        public BlobClientProvider(IOptions<BlobStorageOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }


        public CloudBlobClient Get()
        {
            var connectionString = _options.BlobStorageConnectionString;
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            return storageAccount.CreateCloudBlobClient();
        }
    }
}
